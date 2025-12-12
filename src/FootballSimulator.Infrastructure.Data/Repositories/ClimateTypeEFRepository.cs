using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;

namespace FootballSimulator.Infrastructure.Data
{
    public class ClimateTypeEFRepository : EFLookupRepository<FootballSimulatorDbContext, ClimateType>
    {
        public ClimateTypeEFRepository(FootballSimulatorDbContext context) : base(context)
        {
        }
        protected override IQueryable<ClimateType> EntitySet => base.EntitySet.OrderBy(c => c.Name);
    }
}
