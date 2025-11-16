using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Common.EntityFrameworkCore
{
    //
    // Summary:
    //     Custom base implmentation of Microsoft.EntityFrameworkCore.DbContext that includes
    //     default configuration, helper functions, transaction functions, and custom saving
    //     process. Also implements Common.Core.IUnitOfWork directly for handling transactions
    //     via Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction.
    public abstract class DbContextBase<TContextType> : DbContext, IContextHistorical, IUnitOfWork where TContextType : DbContext
    {
        private readonly IList<EntityHistory> _commandsHistory = new List<EntityHistory>();

        private bool _disposed = false;

        private IDbContextTransaction? _currentTransaction = null;

        //
        // Summary:
        //     Adds the command object to the list of current Common.Core.Domain.EntityHistory
        //     commands performed on the context.
        //
        // Parameters:
        //   command:
        //     The history record representing the command to be stored.
        //
        //   onlyUnique:
        //     Whether only unique commands are allowed. If true and the same Common.Core.Domain.EntityHistory.EntityGuid
        //     and Common.Core.Domain.EntityHistory.TypeId items already exists, it will be
        //     overwritten.
        public virtual void AddToHistory(EntityHistory command, bool onlyUnique = true)
        {
            if (onlyUnique)
            {
                EntityHistory? entityHistory = _commandsHistory.FirstOrDefault(x => x.EntityGuid == command.EntityGuid && x.TypeId == command.TypeId);
                if (entityHistory != null)
                {
                    _commandsHistory.Remove(entityHistory);
                }
            }

            _commandsHistory.Add(command);
        }

        //
        // Summary:
        //     Will trigger Common.EntityFrameworkCore.DbContextBase`1.OnProcessCommandHistories(System.Collections.Generic.IEnumerable{Common.Core.Domain.EntityHistory})
        //     if Common.Core.Domain.EntityHistory commands have been set on the context.
        protected virtual void ProcessAnyCommandHistories()
        {
            if (_commandsHistory.HasItems())
            {
                OnProcessCommandHistories(_commandsHistory);
                _commandsHistory.Clear();
            }
        }

        //
        // Summary:
        //     Called when needing to process/save registered Common.Core.Domain.EntityHistory
        //     commands. By default, this method assumes Microsoft.EntityFrameworkCore.DbSet`1
        //     exists on the context.
        //
        // Parameters:
        //   commandHistories:
        //
        // Exceptions:
        //   T:System.NotImplementedException:
        protected virtual void OnProcessCommandHistories(IEnumerable<EntityHistory> commandHistories)
        {
            if (commandHistories.HasItems())
            {
                Set<EntityHistory>().AddRange(commandHistories);
            }
        }

        //
        // Summary:
        //     Create new instance.
        //
        // Parameters:
        //   options:
        //     Entity Framework's built-in options.
        protected DbContextBase(DbContextOptions<TContextType> options)
            : base(options)
        {
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    base.Dispose();
                }

                DisposeTransaction();
                _disposed = true;
            }
        }

        public override void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~DbContextBase()
        {
            Dispose(disposing: false);
        }

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
                OnSaveChangesError(ex, out var throwOriginalException);
                if (throwOriginalException)
                {
                    throw;
                }
            }

            return 0;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaveChanges();
            try
            {
                int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                OnAfterSaveChanges(result);
                return result;
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                OnSaveChangesError(ex, out var throwOriginalException);
                if (throwOriginalException)
                {
                    throw;
                }
            }

            return 0;
        }

        //
        // Summary:
        //     Event prior to Microsoft.EntityFrameworkCore.DbContext.SaveChanges. Default action
        //     is processing any command histories using Common.Core.Domain.EntityHistory
        //     and validating entries on the ChangeTracker.
        protected virtual void OnBeforeSaveChanges()
        {
            ProcessAnyCommandHistories();
            ValidateEntities();
        }

        //
        // Summary:
        //     Event after Microsoft.EntityFrameworkCore.DbContext.SaveChanges.
        //
        // Parameters:
        //   result:
        //     Result from Microsoft.EntityFrameworkCore.DbContext.SaveChanges.
        protected virtual void OnAfterSaveChanges(int result)
        {
        }

        //
        // Summary:
        //     Called when an exception is thrown attempting to call Microsoft.EntityFrameworkCore.DbContext.SaveChanges(System.Boolean)
        //     or Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(System.Boolean,System.Threading.CancellationToken).
        //     By default, the change tracker Microsoft.EntityFrameworkCore.DbContext.ChangeTracker
        //     is cleared of all attached entities.
        //
        // Parameters:
        //   ex:
        //     Exception thrown during save.
        //
        //   throwOriginalException:
        //     Whether the caught exception in the calling method should be thrown. Defaults
        //     to true.
        protected virtual void OnSaveChangesError(Exception ex, out bool throwOriginalException)
        {
            throwOriginalException = true;
            try
            {
                ChangeTracker.Clear();
            }
            catch (Exception ex2)
            {
                throw new MultiException(
            "There was an error clearing the DbContext ChangeTracker after an exception was thrown on SaveChangesAsync or SaveChanges. See inner exceptions for details.",
            new Exception[] { ex2, ex }
        );

            }
        }

        //
        // Summary:
        //     Validates all new and updated entities found in Microsoft.EntityFrameworkCore.DbContext.ChangeTracker.
        //
        //
        // Exceptions:
        //   T:Common.Core.EntityValidationException:
        protected virtual void ValidateEntities()
        {
            IEnumerable<object> enumerable = from e in ChangeTracker.Entries()
                                             where e.State == EntityState.Added || e.State == EntityState.Modified
                                             select e.Entity;
            List<System.ComponentModel.DataAnnotations.ValidationResult> list = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            List<EntityDataStoreValidationError> list2 = new List<EntityDataStoreValidationError>();
            foreach (object entity in enumerable)
            {
                if (entity is IValidatableObject validatableObject)
                {
                    list.AddRange(validatableObject.Validate(new ValidationContext(entity)));
                }
                else
                {
                    Validator.TryValidateObject(entity, new ValidationContext(entity), list, validateAllProperties: true);
                }

                list2.AddRange(
                    from r in list
                    where r != System.ComponentModel.DataAnnotations.ValidationResult.Success
                    select new EntityDataStoreValidationError(
                        entity.GetType().Name,
                        r.MemberNames.ToSentenceFriendlyText(),
                        r.ErrorMessage ?? string.Empty
                    )
                );

                list.Clear();
            }

            if (list2.HasItems())
            {
                throw new EntityValidationException(list2);
            }
        }

        //
        // Summary:
        //     Initialize a new transaction Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction
        //     for this context.
        public virtual void BeginTransaction()
        {
            _currentTransaction ??= Database.BeginTransaction();
        }

        //
        // Summary:
        //     Initialize a new transaction Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction
        //     for this context.
        public async Task BeginTransactionAsync()
        {
            _currentTransaction ??= await Database.BeginTransactionAsync();
        }

        //
        // Summary:
        //     Saves the current context and commits the transaction if it exists. Rolls back
        //     transaction if exception occurs. Same exception is thrown.
        public virtual void CommitTransaction()
        {
            try
            {
                SaveChanges();
                _currentTransaction?.Commit();
            }
            catch (Exception ex)
            {
                RollbackTransaction(ex);
                throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        //
        // Summary:
        //     Saves the current context and commits the transaction if it exists. Rolls back
        //     transaction if exception occurs. Same exception is thrown.
        public virtual async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                _currentTransaction?.Commit();
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                RollbackTransaction(ex);
                throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        //
        // Summary:
        //     Rolls back the current transaction if it exists.
        public virtual void RollbackTransaction()
        {
            RollbackTransaction(null);
        }

        //
        // Summary:
        //     Rolls back the current transaction if it exists. Optionally include an active
        //     System.Exception ex.
        //
        // Parameters:
        //   ex:
        //     Optional active exception that caused this rollback.
        public virtual void RollbackTransaction(Exception? ex)
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            catch (Exception item)
            {
                if (ex != null)
                {
                    List<Exception> innerExceptions = new List<Exception> { item, ex };
                    throw new MultiException(innerExceptions);
                }

                throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        //
        // Summary:
        //     Performs save on the context. Wrapper for Microsoft.EntityFrameworkCore.DbContext.SaveChanges.
        public virtual void Save()
        {
            SaveChanges();
        }

        //
        // Summary:
        //     Performs save on the context. Wrapper for Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(System.Threading.CancellationToken).
        public virtual Task SaveAsync()
        {
            return SaveChangesAsync();
        }

        //
        // Summary:
        //     Disposes transaction if it exists. Sets local transaction variable to null.
        protected virtual void DisposeTransaction()
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }
}
