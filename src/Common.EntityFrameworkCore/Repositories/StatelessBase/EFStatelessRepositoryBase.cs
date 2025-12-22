using Common.Core;
using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFrameworkCore
{
    public abstract class EFStatelessRepositoryBase<TContext, TType> : EFStatelessRepositoryBase<TContext, TType, int>, IRepository<TType>
    where TContext : DbContext
    where TType : class, IEntity<int>
    {
        protected EFStatelessRepositoryBase(IDbContextFactory<TContext> factory)
            : base(factory)
        {
        }
    }

    public abstract class EFStatelessRepositoryBase<TContext, TType, TKey> : EFStatelessRepositoryBase<TContext, TType, TKey, RepositoryIncludesDefaultOption>, IRepository<TType, TKey>
    where TContext : DbContext
    where TType : class, IEntity<TKey>
    {
        protected EFStatelessRepositoryBase(IDbContextFactory<TContext> factory)
            : base(factory)
        {
        }
    }

    /// <summary>
    /// Entity Framework stateless base repository for a given <see cref="IEntity{TId}"/> and <typeparamref name="TContextType"/>.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TIncludes"></typeparam>
    public abstract class EFStatelessRepositoryBase<TContext, TType, TKey, TIncludes>
    : IRepository<TType, TKey, TIncludes>
    where TContext : DbContext
    where TType : class, IEntity<TKey>
    where TIncludes : struct, Enum
    {
        protected readonly IDbContextFactory<TContext> Factory;

        protected EFStatelessRepositoryBase(IDbContextFactory<TContext> factory)
        {
            Factory = factory;
        }

        protected virtual IQueryable<TType> BuildFullEntitySet(TContext db)
        {
            // derived classes override this to add includes
            return db.Set<TType>();
        }

        // ------------------------------------------------------------
        // Queryable builder (override in derived classes for includes)
        // ------------------------------------------------------------
        protected virtual IQueryable<TType> BuildQueryable(TContext db, TIncludes includes)
        {
            return db.Set<TType>();
        }

        // ------------------------------------------------------------
        // GetById
        // ------------------------------------------------------------
        public virtual TType GetById(TKey id, TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, includes)
                .FirstOrDefault(x => x.Id.Equals(id));
        }

        public virtual async Task<TType> GetByIdAsync(TKey id, TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, includes)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        // ------------------------------------------------------------
        // GetAll
        // ------------------------------------------------------------
        public virtual IEnumerable<TType> GetAll(TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, includes).ToList();
        }

        public virtual async Task<IEnumerable<TType>> GetAllAsync(TIncludes includes = default)
        {
            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, includes).ToListAsync();
        }

        public virtual IEnumerable<TType> GetAll(IEnumerable<TKey> ids, TIncludes includes = default)
        {
            if (ids == null || !ids.Any())
                return Enumerable.Empty<TType>();

            using var db = Factory.CreateDbContext();
            return BuildQueryable(db, includes)
                .Where(x => ids.Contains(x.Id))
                .ToList();
        }

        public virtual async Task<IEnumerable<TType>> GetAllAsync(IEnumerable<TKey> ids, TIncludes includes = default)
        {
            if (ids == null || !ids.Any())
                return Enumerable.Empty<TType>();

            using var db = Factory.CreateDbContext();
            return await BuildQueryable(db, includes)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        // ------------------------------------------------------------
        // Count
        // ------------------------------------------------------------
        public virtual int Count()
        {
            using var db = Factory.CreateDbContext();
            return db.Set<TType>().Count();
        }

        public virtual async Task<int> CountAsync()
        {
            using var db = Factory.CreateDbContext();
            return await db.Set<TType>().CountAsync();
        }

        // ------------------------------------------------------------
        // AddOrUpdate
        // ------------------------------------------------------------
        public virtual void AddOrUpdate(TType item, int userId = 0, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            if (item.IsNew)
                db.Set<TType>().Add(item);
            else
                db.Set<TType>().Update(item);

            db.SaveChanges();
        }

        public virtual async Task AddOrUpdateAsync(TType item, int userId = 0, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            if (item.IsNew)
                await db.Set<TType>().AddAsync(item);
            else
                db.Set<TType>().Update(item);

            await db.SaveChangesAsync();
        }

        // ------------------------------------------------------------
        // Update with callback
        // ------------------------------------------------------------
        public virtual TType Update(TKey id, Action<TType> callback, TIncludes includes = default, int userId = 0, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            var entity = BuildQueryable(db, includes)
                .FirstOrDefault(x => x.Id.Equals(id))
                ?? throw new DataObjectNotFoundException(typeof(TType).Name, $"Id: {id}");

            callback(entity);

            db.Set<TType>().Update(entity);
            db.SaveChanges();

            return entity;
        }

        public virtual async Task<TType> UpdateAsync(TKey id, Action<TType> callback, TIncludes includes = default, int userId = 0, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            var entity = await BuildQueryable(db, includes)
                .FirstOrDefaultAsync(x => x.Id.Equals(id))
                ?? throw new DataObjectNotFoundException(typeof(TType).Name, $"Id: {id}");

            callback(entity);

            db.Set<TType>().Update(entity);
            await db.SaveChangesAsync();

            return entity;
        }

        // ------------------------------------------------------------
        // Delete
        // ------------------------------------------------------------
        public virtual void Delete(TType item, int userId = 0, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();
            db.Set<TType>().Remove(item);
            db.SaveChanges();
        }

        public virtual async Task DeleteAsync(TType item, int userId = 0, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();
            db.Set<TType>().Remove(item);
            await db.SaveChangesAsync();
        }

        public virtual void Delete(TKey id, int userId = 0, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            var entity = db.Set<TType>().FirstOrDefault(x => x.Id.Equals(id))
                ?? throw new DataObjectNotFoundException(typeof(TType).Name, $"Id: {id}");

            db.Set<TType>().Remove(entity);
            db.SaveChanges();
        }

        public virtual async Task DeleteAsync(TKey id, int userId = 0, bool? recordChangeEvent = null)
        {
            using var db = Factory.CreateDbContext();

            var entity = await db.Set<TType>().FirstOrDefaultAsync(x => x.Id.Equals(id))
                ?? throw new DataObjectNotFoundException(typeof(TType).Name, $"Id: {id}");

            db.Set<TType>().Remove(entity);
            await db.SaveChangesAsync();
        }
    }
}