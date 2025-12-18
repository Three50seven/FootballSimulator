using Common.Core;

namespace FootballSimulator.Application.Services
{
    public abstract class EditServiceBase<TEntity, TRepository>
    where TEntity : class
    where TRepository : IRepository<TEntity>
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly TRepository _repository;

        protected EditServiceBase(TRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<TEntity> AddOrUpdateAsync(TEntity entity, int userId = default)
        {
            await _repository.AddOrUpdateAsync(entity, userId);

            // Centralized commit boundary
            await _unitOfWork.SaveAsync();

            return entity;
        }

        public virtual async Task<TEntity> AddOrUpdateWithTransactionAsync(TEntity entity, int userId = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _repository.AddOrUpdateAsync(entity, userId);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction(ex);
                throw;
            }
        }

        public virtual async Task DeleteAsync(TEntity entity, int userId = default)
        {
            await _repository.DeleteAsync(entity, userId);
            // Centralized commit boundary
            await _unitOfWork.SaveAsync();
        }
    }

    public abstract class EditServiceBase<TEntity, TRepository, TIncludes>
    where TEntity : class
    where TIncludes : struct, Enum
    where TRepository : IDomainRepository<TEntity, TIncludes>
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly TRepository _repository;

        protected EditServiceBase(TRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        // Common methods (no includes required)
        public virtual async Task<TEntity> AddOrUpdateAsync(TEntity entity, int userId = default)
        {
            await _repository.AddOrUpdateAsync(entity, userId);
            await _unitOfWork.SaveAsync();
            return entity;
        }

        public virtual async Task<TEntity> AddOrUpdateWithTransactionAsync(TEntity entity, int userId = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _repository.AddOrUpdateAsync(entity, userId);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction(ex);
                throw;
            }
        }

        public virtual async Task DeleteAsync(TEntity entity, int userId = default)
        {
            await _repository.DeleteAsync(entity, userId);
            await _unitOfWork.SaveAsync();
        }

        // Include-aware methods
        public virtual async Task<TEntity?> GetByGuidAsync(Guid guid, TIncludes includes = default)
        {
            return await _repository.GetByGuidAsync(guid, includes);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<Guid> guids, TIncludes includes = default)
        {
            return await _repository.GetAllAsync(guids, includes);
        }
    }
}