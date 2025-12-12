using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;

namespace FootballSimulator.Infrastructure.Data
{
    public class CountryEFRepository : EFLookupRepository<FootballSimulatorDbContext, Country>
    {
        public CountryEFRepository(FootballSimulatorDbContext context) : base(context)
        {
        }
        protected override IQueryable<Country> EntitySet => base.EntitySet.OrderBy(c => c.Name);
    }
}
