using Microsoft.Extensions.DependencyInjection;
using Common.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Base class for adapter-link pattern for setting up standard scoped repositories by using a new scope per method call.
    /// </summary>
    /// <typeparam name="TRepository"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ScopedRepositoryAdapterBase<TRepository, TEntity>
        : ScopedRepositoryAdapterBase<TRepository, TEntity, int>, IRepository<TEntity>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class
    {
        protected ScopedRepositoryAdapterBase(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        {
        }
    }

    /// <summary>
    /// Base class for adapter-link pattern for setting up standard scoped repositories by using a new scope per method call.
    /// </summary>
    /// <typeparam name="TRepository"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class ScopedRepositoryAdapterBase<TRepository, TEntity, TKey> 
        : ScopedRepositoryAdapterBase<TRepository, TEntity, TKey, RepositoryIncludesDefaultOption>, IRepository<TEntity, TKey>
        where TRepository : class, IRepository<TEntity, TKey>
        where TEntity : class
    {
        protected ScopedRepositoryAdapterBase(IServiceScopeFactory serviceScopeFactory) 
            : base(serviceScopeFactory)
        {
        }
    }

    /// <summary>
    /// Base class for adapter-link pattern for setting up standard scoped repositories by using a new scope per method call.
    /// </summary>
    /// <typeparam name="TRepository"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TIncludes"></typeparam>
    public abstract class ScopedRepositoryAdapterBase<TRepository, TEntity, TKey, TIncludes> 
        : ScopedRepositoryAdapterBase<TRepository>, IRepository<TEntity, TKey, TIncludes>
        where TRepository : class, IRepository<TEntity, TKey, TIncludes>
        where TEntity : class
        where TIncludes : struct, Enum
    {
        public ScopedRepositoryAdapterBase(IServiceScopeFactory serviceScopeFactory)
            : base (serviceScopeFactory)
        {

        }

        public virtual void AddOrUpdate(TEntity item, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                GetRepository(scope).AddOrUpdate(item, userId, recordChangeEvent);
                GetUnitOfWork(scope).Save();
            }
        }

        public virtual async Task AddOrUpdateAsync(TEntity item, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                await GetRepository(scope).AddOrUpdateAsync(item, userId, recordChangeEvent);
                await GetUnitOfWork(scope).SaveAsync();
            }
        }

        public virtual void AddOrUpdateMany(IEnumerable<TEntity> items, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                GetRepository(scope).AddOrUpdateMany(items, userId, recordChangeEvent);
                GetUnitOfWork(scope).Save();
            }
        }

        public virtual async Task AddOrUpdateManyAsync(IEnumerable<TEntity> items, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                await GetRepository(scope).AddOrUpdateManyAsync(items, userId, recordChangeEvent);
                await GetUnitOfWork(scope).SaveAsync();
            }
        }

        public TEntity Update(TKey id, Action<TEntity> callback, TIncludes includes = default, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                var entity = GetRepository(scope).Update(id, callback, includes, userId, recordChangeEvent);
                GetUnitOfWork(scope).Save();
                return entity;
            }
        }

        public async Task<TEntity> UpdateAsync(TKey id, Action<TEntity> callback, TIncludes includes = default, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                var entity = await GetRepository(scope).UpdateAsync(id, callback, includes, userId, recordChangeEvent);
                await GetUnitOfWork(scope).SaveAsync();
                return entity;
            }
        }

        public virtual int Count()
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).Count();
            }
        }

        public virtual async Task<int> CountAsync()
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).CountAsync();
            }
        }

        public virtual void Delete(TEntity item, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                GetRepository(scope).Delete(item, userId, recordChangeEvent);
                GetUnitOfWork(scope).Save();
            }
        }

        public virtual async Task DeleteAsync(TEntity item, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                await GetRepository(scope).DeleteAsync(item, userId, recordChangeEvent);
                await GetUnitOfWork(scope).SaveAsync();
            }
        }

        public void Delete(TKey id, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                GetRepository(scope).Delete(id, userId);
                GetUnitOfWork(scope).Save();
            }
        }

        public async Task DeleteAsync(TKey id, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                await GetRepository(scope).DeleteAsync(id, userId, recordChangeEvent);
                await GetUnitOfWork(scope).SaveAsync();
            }
        }

        public virtual void DeleteMany(IEnumerable<TEntity> items, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                GetRepository(scope).DeleteMany(items, userId, recordChangeEvent);
                GetUnitOfWork(scope).Save();
            }
        }

        public virtual async Task DeleteManyAsync(IEnumerable<TEntity> items, int userId = 0, bool? recordChangeEvent = null)
        {
            using (var scope = CreateScope())
            {
                await GetRepository(scope).DeleteManyAsync(items, userId, recordChangeEvent);
                await GetUnitOfWork(scope).SaveAsync();
            }
        }

        public virtual IEnumerable<TEntity> GetAll(TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetAll(includes);
            }
        }

        public virtual IEnumerable<TEntity> GetAll(IEnumerable<TKey> ids, TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetAll(ids, includes);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetAllAsync(includes);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<TKey> ids, TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetAllAsync(ids, includes);
            }
        }

        public virtual TEntity GetById(TKey id, TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetById(id, includes);
            }
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id, TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetByIdAsync(id, includes);
            }
        }        
    }

    /// <summary>
    /// Base class for adapter-link pattern for setting up standard scoped repositories by using a new scope per method call.
    /// </summary>
    /// <typeparam name="TRepository"></typeparam>
    public abstract class ScopedRepositoryAdapterBase<TRepository> : ScopedAdapterBase<TRepository>
        where TRepository : class
    {
        public ScopedRepositoryAdapterBase(IServiceScopeFactory serviceScopeFactory)
            : base (serviceScopeFactory)
        {

        }

        protected override void ValidateRegisteredServices(IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                GetRepository(scope);
                GetUnitOfWork(scope);
            }
        }

        protected static TRepository GetRepository(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<TRepository>();
        protected static IUnitOfWork GetUnitOfWork(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }
}