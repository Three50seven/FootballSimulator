using Common.Core;
using Common.Core.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Common.EntityFrameworkCore
{
    public abstract partial class DbContextBase<TContextType> : DbContext
        where TContextType : DbContext
    {
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaveChanges();

            try
            {
                int result = base.SaveChanges(acceptAllChangesOnSuccess);

                OnAfterSaveChanges(result);

                return result;
            }
            catch (Exception ex)
            {
                OnSaveChangesError(ex, out bool throwOriginalException);
                if (throwOriginalException)
                    throw;
            }

            return 0;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();

            try
            {
                int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                OnAfterSaveChanges(result);
                return result;
            }
            catch (Exception ex)
            {
                OnSaveChangesError(ex, out bool throwOriginalException);
                if (throwOriginalException)
                    throw;
            }

            return 0;
        }

        /// <summary>
        /// Event prior to <see cref="DbContext.SaveChanges()"/>.
        /// Default action is processing any command histories using <see cref="Common.Core.Domain.EntityHistory"/> and validating entries on the ChangeTracker.
        /// </summary>
        protected virtual void OnBeforeSaveChanges()
        {
            ProcessAnyCommandHistories();
            ValidateEntities();
        }

        /// <summary>
        /// Event after <see cref="DbContext.SaveChanges()"/>.
        /// </summary>
        /// <param name="result">Result from <see cref="DbContext.SaveChanges()"/>.</param>
        protected virtual void OnAfterSaveChanges(int result) { }

        /// <summary>
        /// Called when an exception is thrown attempting to call <see cref="DbContext.SaveChanges(bool)"/> or <see cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/>.
        /// By default, the change tracker <see cref="DbContext.ChangeTracker"/> is cleared of all attached entities.
        /// </summary>
        /// <param name="ex">Exception thrown during save.</param>
        /// <param name="throwOriginalException">Whether the caught exception in the calling method should be thrown. Defaults to true.</param>
        protected virtual void OnSaveChangesError(Exception ex, out bool throwOriginalException)
        {
            throwOriginalException = true;

            try
            {
                ChangeTracker.Clear();
            }
            catch (Exception clearingEx)
            {
                throw new MultiException("There was an error clearing the DbContext ChangeTracker after an exception was thrown on SaveChangesAsync or SaveChanges. See inner exceptions for details.",
                                         [clearingEx, ex]);
            }
        }

        /// <summary>
        /// Validates all new and updated entities found in <see cref="DbContext.ChangeTracker"/>.
        /// </summary>
        /// <exception cref="EntityValidationException"></exception>
        protected virtual void ValidateEntities()
        {
            var entities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).Select(e => e.Entity);

            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var validationErrors = new List<EntityDataStoreValidationError>();

            foreach (var entity in entities)
            {
                // perform validation on the entity via direct interface implementation or through static Validator
                // AFAIK, only attribute validation is used here, but this extra validation check doesn't hurt
                if (entity is IValidatableObject validatable)
                    validationResults.AddRange(validatable.Validate(new ValidationContext(entity)));
                else
                    Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, validateAllProperties: true);

                // get any failed validation results and build domain-based validation error class
                // NOTE: some guesswork was done here - this logic may be incorrect in presenting the correct values
                validationErrors.AddRange(
                    validationResults
                    .Where(r => r != System.ComponentModel.DataAnnotations.ValidationResult.Success)
                    .Select(r => new EntityDataStoreValidationError(
                        entity.GetType().Name,
                        r.MemberNames.ToSentenceFriendlyText(),
                        r.ErrorMessage ?? string.Empty)));

                validationResults.Clear();
            }

            if (validationErrors.HasItems())
                throw new EntityValidationException(validationErrors);
        }
    }
}
