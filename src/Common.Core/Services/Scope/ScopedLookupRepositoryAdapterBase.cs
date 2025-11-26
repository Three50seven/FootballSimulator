using Microsoft.Extensions.DependencyInjection;
using Common.Core.Domain;
using Common.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Core
{
    public abstract class ScopedLookupRepositoryAdapterBase<TRepository, TType> 
        : ScopedRepositoryAdapterBase<TRepository>, ILookupRepository<TType>
        where TRepository : class, ILookupRepository<TType>
        where TType : class, ILookupEntity
    {
        public ScopedLookupRepositoryAdapterBase(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        {

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

        public virtual IEnumerable<TType> GetAll(RepositoryIncludesDefaultOption includes = default)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetAll(includes);
            }
        }

        public virtual IEnumerable<TType> GetAll(IEnumerable<int> ids, RepositoryIncludesDefaultOption includes = default)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetAll(includes);
            }
        }

        public virtual async Task<IEnumerable<TType>> GetAllAsync(RepositoryIncludesDefaultOption includes = default)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetAllAsync(includes);
            }
        }

        public virtual async Task<IEnumerable<TType>> GetAllAsync(IEnumerable<int> ids, RepositoryIncludesDefaultOption includes = default)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetAllAsync(ids, includes);
            }
        }

        public virtual TType GetById(int id, RepositoryIncludesDefaultOption includes = default)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetById(id, includes);
            }
        }

        public virtual async Task<TType> GetByIdAsync(int id, RepositoryIncludesDefaultOption includes = default)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetByIdAsync(id, includes);
            }
        }

        public virtual TType GetByName(string name)
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetByName(name);
            }
        }

        public virtual async Task<TType> GetByNameAsync(string name)
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetByNameAsync(name);
            }
        }

        public virtual IEnumerable<SelectItem> GetSelections()
        {
            using (var scope = CreateScope())
            {
                return GetRepository(scope).GetSelections();
            }
        }

        public virtual async Task<IEnumerable<SelectItem>> GetSelectionsAsync()
        {
            using (var scope = CreateScope())
            {
                return await GetRepository(scope).GetSelectionsAsync();
            }
        }
    }
}
