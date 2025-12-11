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
}
