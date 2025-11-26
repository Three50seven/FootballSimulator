using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    public class EntityCopyService<T> : IEntityCopyService<T>
        where T : class, IDomainEntity, ICopyable<T>
    {
        public EntityCopyService(IDomainRepository<T> repository)
        {
            Repository = repository;
        }

        protected IDomainRepository<T> Repository { get; }

        public virtual CommandResult Copy(Guid id)
        {
            try
            {
                var existingEntity = Repository.GetByGuid(id);
                if (existingEntity == null)
                    throw new DataObjectNotFoundException(nameof(T), id);

                var copiedEntity = existingEntity.Copy();
                if (copiedEntity == null)
                    throw new InvalidOperationException($"Copied entity {typeof(T).FullName} is null.");

                Repository.AddOrUpdate(copiedEntity);

                return CommandResult.Success();
            }
            catch (ValidationException vex)
            {
                return CommandResult.Fail(vex.BrokenRules);
            }
        }

        public virtual async Task<CommandResult> CopyAsync(Guid id)
        {
            try
            {
                var existingEntity = await Repository.GetByGuidAsync(id);
                if (existingEntity == null)
                    throw new DataObjectNotFoundException(nameof(T), id);

                var copiedEntity = existingEntity.Copy();
                if (copiedEntity == null)
                    throw new InvalidOperationException($"Copied entity {typeof(T).FullName} is null.");

                await Repository.AddOrUpdateAsync(copiedEntity);

                return CommandResult.Success();
            }
            catch (ValidationException vex)
            {
                return CommandResult.Fail(vex.BrokenRules);
            }
        }
    }
}
