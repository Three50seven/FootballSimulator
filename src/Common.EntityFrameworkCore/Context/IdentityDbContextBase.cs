using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFrameworkCore
{
    /// <summary>
    /// Custom Identity DbContext base class with support for entity history tracking and IUnitOfWork.
    /// </summary>
    /// <typeparam name="TUser">custom user class (e.g., ApplicationUser)</typeparam>
    /// <typeparam name="TKey">The primary key type for the user (usually string or Guid)</typeparam>
    public abstract partial class IdentityDbContextBase<TUser, TKey> :
        IdentityDbContext<TUser, IdentityRole<TKey>, TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private bool _disposed = false;

        protected IdentityDbContextBase(DbContextOptions options) : base(options) { }        

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

        ~IdentityDbContextBase() // the finalizer
        {
            Dispose(false);
        }
    }
}
