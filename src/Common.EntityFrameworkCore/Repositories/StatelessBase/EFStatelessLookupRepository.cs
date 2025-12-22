using Common.Core;
using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFrameworkCore
{
    public class EFStatelessLookupRepository<TContextType, TType> : EFStatelessRepositoryBase<TContextType, TType>, ILookupRepository<TType>
    where TContextType : DbContext
    where TType : class, ILookupEntity
    {
        public EFStatelessLookupRepository(IDbContextFactory<TContextType> factory)
            : base(factory)
        {

        }

        // ------------------------------------------------------------
        // Queryable builder (ordered by Name)
        // ------------------------------------------------------------
        protected virtual IQueryable<TType> BuildQueryable(TContextType db)
        {
            return db.Set<TType>().OrderBy(x => x.Name);
        }

        // ------------------------------------------------------------
        // GetByName
        // ------------------------------------------------------------
        public virtual TType GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            using var db = Factory.CreateDbContext();

            return BuildQueryable(db)
                .FirstOrDefault(x =>
                    x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual async Task<TType> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            using var db = Factory.CreateDbContext();

            return await BuildQueryable(db)
                .FirstOrDefaultAsync(x =>
                    x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        // ------------------------------------------------------------
        // GetSelections
        // ------------------------------------------------------------
        public virtual IEnumerable<SelectItem> GetSelections()
        {
            using var db = Factory.CreateDbContext();

            return BuildQueryable(db)
                .Select(x => new { x.Id, x.Name })
                .ToList() // hit database
                .Select(x => new SelectItem(x.Id, x.Name));
        }

        public virtual async Task<IEnumerable<SelectItem>> GetSelectionsAsync()
        {
            using var db = Factory.CreateDbContext();

            var data = await BuildQueryable(db)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();

            return data.Select(x => new SelectItem(x.Id, x.Name));
        }
    }
}
