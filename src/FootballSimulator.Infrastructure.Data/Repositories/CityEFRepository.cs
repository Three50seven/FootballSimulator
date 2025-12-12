using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public class CityEFRepository : EFLookupRepository<FootballSimulatorDbContext, City>
    {
        public CityEFRepository(FootballSimulatorDbContext context) : base(context)
        {
        }

        protected override IQueryable<City> EntitySet => base.EntitySet.Include(c => c.State);
    }
}
