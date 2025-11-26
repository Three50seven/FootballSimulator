using Microsoft.Extensions.DependencyInjection;
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
    public abstract class ScopedDomainRepositoryAdapterBase<TRepository, TEntity>
        : ScopedDomainRepositoryAdapterBase<TRepository, TEntity, RepositoryIncludesDefaultOption>, IDomainRepository<TEntity>
        where TRepository : class, IDomainRepository<TEntity>
        where TEntity : class
    {
        protected ScopedDomainRepositoryAdapterBase(IServiceScopeFactory serviceScopeFactory) 
            : base(serviceScopeFactory)
        {
        }
    }

    /// <summary>
    /// Base class for adapter-link pattern for setting up standard scoped repositories by using a new scope per method call.
    /// </summary>
    /// <typeparam name="TRepository"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIncludes"></typeparam>
    public abstract class ScopedDomainRepositoryAdapterBase<TRepository, TEntity, TIncludes> 
        : ScopedRepositoryAdapterBase<TRepository, TEntity, int, TIncludes>, IDomainRepository<TEntity, TIncludes>
        where TRepository : class, IDomainRepository<TEntity, TIncludes>
        where TEntity : class
        where TIncludes : struct, Enum
    {
        protected ScopedDomainRepositoryAdapterBase(IServiceScopeFactory serviceScopeFactory) 
            : base(serviceScopeFactory)
        {
        }

        public virtual IEnumerable<TEntity> GetAll(IEnumerable<Guid> guids, TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetAll(guids, includes);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<Guid> guids, TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetAllAsync(guids, includes);
            }
        }

        public virtual TEntity GetByGuid(Guid guid, TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetByGuid(guid, includes);
            }
        }

        public virtual async Task<TEntity> GetByGuidAsync(Guid guid, TIncludes includes = default)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetByGuidAsync(guid, includes);
            }
        }

        public virtual Guid GetGuid(int id)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetGuid(id);
            }
        }

        public virtual async Task<Guid> GetGuidAsync(int id)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetGuidAsync(id);
            }
        }

        public virtual int GetId(Guid guid)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetId(guid);
            }
        }

        public virtual async Task<int> GetIdAsync(Guid guid)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetIdAsync(guid);
            }
        }
    }
}