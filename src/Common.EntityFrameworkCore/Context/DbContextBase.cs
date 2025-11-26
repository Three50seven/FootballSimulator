using Microsoft.EntityFrameworkCore;

namespace Common.EntityFrameworkCore
{
    /// <summary>
    /// Custom base implmentation of <see cref="DbContext"/> that includes default configuration, helper functions, transaction functions, and custom saving process.
    /// Also implements <see cref="Common.Core.IUnitOfWork"/> directly for handling transactions via <see cref="Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction"/>.
    /// </summary>
    public abstract partial class DbContextBase<TContextType> : DbContext
        where TContextType : DbContext
    {
        private bool _disposed = false;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="options">Entity Framework's built-in options.</param>
        protected DbContextBase(DbContextOptions<TContextType> options)
            : base(options)
        {

        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    base.Dispose();

                DisposeTransaction();
                _disposed = true;
            }
        }
        public override void Dispose() // Implement IDisposable
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DbContextBase() // the finalizer
        {
            Dispose(false);
        }
    }
}
