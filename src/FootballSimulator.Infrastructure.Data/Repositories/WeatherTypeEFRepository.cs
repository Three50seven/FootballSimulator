using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;

namespace FootballSimulator.Infrastructure.Data
{
    public class WeatherTypeEFRepository : EFLookupRepository<FootballSimulatorDbContext, WeatherType>
    {
        public WeatherTypeEFRepository(FootballSimulatorDbContext context) : base(context)
        {
        }
        protected override IQueryable<WeatherType> EntitySet => base.EntitySet.OrderBy(wt => wt.Name);    
    }
}
