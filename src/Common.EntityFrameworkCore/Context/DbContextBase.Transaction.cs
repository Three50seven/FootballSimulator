using Common.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Common.EntityFrameworkCore
{
    public abstract partial class DbContextBase<TContextType> : DbContext, IUnitOfWork
        where TContextType : DbContext
    {
        private IDbContextTransaction? _currentTransaction = null;

        /// <summary>
        /// Initialize a new transaction <see cref="IDbContextTransaction"/> for this context.
        /// </summary>
        public virtual void BeginTransaction()
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Initialize a new transaction <see cref="IDbContextTransaction"/> for this context.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = await Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Saves the current context and commits the transaction if it exists.
        /// Rolls back transaction if exception occurs. Same exception is thrown.
        /// </summary>
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

        /// <summary>
        /// Saves the current context and commits the transaction if it exists.
        /// Rolls back transaction if exception occurs. Same exception is thrown.
        /// </summary>
        /// <returns></returns>
        public virtual async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
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

        /// <summary>
        /// Rolls back the current transaction if it exists.
        /// </summary>
        public virtual void RollbackTransaction()
        {
            RollbackTransaction(null);
        }

        /// <summary>
        /// Rolls back the current transaction if it exists. Optionally include an active <see cref="Exception"/> <paramref name="ex"/>.
        /// </summary>
        /// <param name="ex">Optional active exception that caused this rollback.</param>
        public virtual void RollbackTransaction(Exception? ex)
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            catch (Exception transactionEx)
            {
                if (ex != null)
                {
                    var exceptions = new List<Exception>()
                    {
                        transactionEx,
                        ex
                    };
                    throw new MultiException(exceptions);
                }
                else
                    throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        /// <summary>
        /// Performs save on the context. Wrapper for <see cref="DbContext.SaveChanges()"/>.
        /// </summary>
        public virtual void Save()
        {
            SaveChanges();
        }

        /// <summary>
        /// Performs save on the context. Wrapper for <see cref="DbContext.SaveChangesAsync(CancellationToken)"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Task SaveAsync()
        {
            return SaveChangesAsync();
        }

        /// <summary>
        /// Disposes transaction if it exists. Sets local transaction variable to null.
        /// </summary>
        protected virtual void DisposeTransaction()
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }
}
