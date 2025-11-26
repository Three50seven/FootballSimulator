using Microsoft.EntityFrameworkCore;
using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.EntityFrameworkCore
{
    /// <summary>
    /// Entity Framework base repository for a given <see cref="IEntity{TId}"/> and <typeparamref name="TContextType"/>.
    /// Implements <see cref="IRepository{TType}"/> with generic Entity Framework functionality.
    /// </summary>
    /// <typeparam name="TContextType"></typeparam>
    /// <typeparam name="TType"></typeparam>
    public abstract class EFRepositoryBase<TContextType, TType> : EFRepositoryBase<TContextType, TType, int>, IRepository<TType>
        where TContextType : DbContext
        where TType : class, IEntity<int>
    {
        protected EFRepositoryBase(TContextType context)
            : base(context)
        {
        }
    }

    /// <summary>
    /// Entity Framework base repository for a given <see cref="IEntity{TId}"/> and <typeparamref name="TContextType"/>.
    /// Implements <see cref="IRepository{TType, TKey}"/> with generic Entity Framework functionality.
    /// </summary>
    /// <typeparam name="TContextType"></typeparam>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EFRepositoryBase<TContextType, TType, TKey> : EFRepositoryBase<TContextType, TType, TKey, RepositoryIncludesDefaultOption>, IRepository<TType, TKey>
        where TContextType : DbContext
        where TType : class, IEntity<TKey>
    {
        protected EFRepositoryBase(TContextType context)
            : base(context)
        {
        }
    }

    /// <summary>
    /// Entity Framework base repository for a given <see cref="IEntity{TId}"/> and <typeparamref name="TContextType"/>.
    /// Implements <see cref="IRepository{TType, TKey}"/> with generic Entity Framework functionality.
    /// </summary>
    /// <typeparam name="TContextType"></typeparam>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TIncludes"></typeparam>
    public abstract class EFRepositoryBase<TContextType, TType, TKey, TIncludes> : IRepository<TType, TKey, TIncludes>
        where TContextType : DbContext
        where TType : class, IEntity<TKey>
        where TIncludes : struct, Enum
    {        
        protected EFRepositoryBase(TContextType context)
        {
            Guard.IsNotNull(context, nameof(context));

            DbSet = context.Set<TType>();
            Context = context;
        }

        protected virtual DbSet<TType> DbSet { get; private set; }
        protected virtual TContextType Context { get; private set; }
        protected virtual IQueryable<TType> EntitySet => DbSet;

        protected virtual IQueryable<TType> GetEntitySet(TIncludes includes)
        {
            return EntitySet;
        }

        public virtual TType GetById(TKey id, TIncludes includes = default)
        {
            return GetEntitySet(includes).FirstOrDefault(DbSetExtensions.IdEqualsPredicate<TType, TKey>(id))!;
        }

        public virtual Task<TType> GetByIdAsync(TKey id, TIncludes includes = default)
        {
            return GetEntitySet(includes).FirstOrDefaultAsync(DbSetExtensions.IdEqualsPredicate<TType, TKey>(id))!;
        }

        public virtual IEnumerable<TType> GetAll(TIncludes includes = default)
        {
            return GetEntitySet(includes).ToList();
        }

        public virtual async Task<IEnumerable<TType>> GetAllAsync(TIncludes includes = default)
        {
            return await GetEntitySet(includes).ToListAsync();
        }

        public virtual IEnumerable<TType> GetAll(IEnumerable<TKey> ids, TIncludes includes = default)
        {
            if (ids == null || !ids.Any())
                return null!;

            return GetEntitySet(includes).Where(x => ids.Contains(x.Id)).ToList();
        }

        public virtual async Task<IEnumerable<TType>> GetAllAsync(IEnumerable<TKey> ids, TIncludes includes = default)
        {
            if (ids == null || !ids.Any())
                return null!;

            return await GetEntitySet(includes).Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual int Count()
        {
            return EntitySet.Select(x => x.Id).Count();
        }

        public virtual Task<int> CountAsync()
        {
            return EntitySet.Select(x => x.Id).CountAsync();
        }

        public virtual void AddOrUpdate(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            Guard.IsNotNull(item, nameof(item));

            if (item.IsNew)
                DbSet.Add(item);
            else
                DbSet.Update(item);
        }

        public virtual async Task AddOrUpdateAsync(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            Guard.IsNotNull(item, nameof(item));

            if (item.IsNew)
                await DbSet.AddAsync(item);
            else
                DbSet.Update(item);
        }

        public virtual TType Update(TKey id, Action<TType> callback, TIncludes includes = default, int userId = 0, bool? recordChangeEvent = null)
        {
            Guard.IsNotNull(callback, nameof(callback));

            var entity = GetById(id, includes) ?? throw new DataObjectNotFoundException(typeof(TType).Name, $"Id: {id}");
            callback.Invoke(entity);
            AddOrUpdate(entity, userId, recordChangeEvent);

            return entity;
        }

        public virtual async Task<TType> UpdateAsync(TKey id, Action<TType> callback, TIncludes includes = default, int userId = 0, bool? recordChangeEvent = null)
        {
            Guard.IsNotNull(callback, nameof(callback));

            var entity = await GetByIdAsync(id, includes) ?? throw new DataObjectNotFoundException(typeof(TType).Name, $"Id: {id}");
            callback.Invoke(entity);
            await AddOrUpdateAsync(entity, userId, recordChangeEvent);

            return entity;
        }

        public virtual void Delete(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            Guard.IsNotNull(item, nameof(item));
            DbSet.Remove(item);
        }

        public virtual Task DeleteAsync(TType item, int userId = default, bool? recordChangeEvent = null)
        {
            Delete(item, userId, recordChangeEvent);
            return Task.CompletedTask;
        }

        public virtual void Delete(TKey id, int userId = 0, bool? recordChangeEvent = null)
        {
            var entity = DbSet.FirstOrDefault(DbSetExtensions.IdEqualsPredicate<TType, TKey>(id)) ?? throw new DataObjectNotFoundException(typeof(TType).Name, $"Id: {id}");
            Delete(entity, userId, recordChangeEvent);
        }

        public virtual async Task DeleteAsync(TKey id, int userId = 0, bool? recordChangeEvent = null)
        {
            var entity = await DbSet.FirstOrDefaultAsync(DbSetExtensions.IdEqualsPredicate<TType, TKey>(id)) ?? throw new DataObjectNotFoundException(typeof(TType).Name, $"Id: {id}");
            await DeleteAsync(entity, userId, recordChangeEvent);
        }
    }
}
