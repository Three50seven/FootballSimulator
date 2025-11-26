using Microsoft.EntityFrameworkCore;
using Common.Core;
using Common.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.EntityFrameworkCore
{
    /// <summary>
    /// Repository designed against standard <see cref="ILookupEntity"/> entities.
    /// </summary>
    /// <typeparam name="TContextType"></typeparam>
    /// <typeparam name="TType"></typeparam>
    public class EFLookupRepository<TContextType, TType> : EFRepositoryBase<TContextType, TType>, ILookupRepository<TType>
        where TContextType : DbContext
        where TType : class, ILookupEntity
    {
        public EFLookupRepository(TContextType context)
            : base(context)
        {

        }

        protected override IQueryable<TType> EntitySet => base.EntitySet.OrderBy(x => x.Name);

        public virtual TType GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return EntitySet.FirstOrDefault(x => x.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual Task<TType> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Task.FromResult<TType>(null);

            return EntitySet.FirstOrDefaultAsync(x => x.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual IEnumerable<SelectItem> GetSelections()
        {
            return EntitySet
               .Select(x => new { x.Id, x.Name })
               .ToList() // hit database
               .Select(x => (new SelectItem(x.Id, x.Name)));
        }

        public virtual async Task<IEnumerable<SelectItem>> GetSelectionsAsync()
        {
            var data = await EntitySet
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();

            return data.Select(x => new SelectItem(x.Id, x.Name));
        }
    }
}
