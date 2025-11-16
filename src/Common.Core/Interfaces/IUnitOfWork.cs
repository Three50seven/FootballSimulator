namespace Common.Core
{
    public interface IUnitOfWork
    {
        void Save();

        Task SaveAsync();

        void BeginTransaction();

        Task BeginTransactionAsync();

        void CommitTransaction();

        Task CommitTransactionAsync();

        void RollbackTransaction();

        void RollbackTransaction(Exception ex);
    }
}
