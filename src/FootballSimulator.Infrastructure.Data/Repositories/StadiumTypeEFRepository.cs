using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;

namespace FootballSimulator.Infrastructure.Data
{
    public class StadiumTypeEFRepository : EFLookupRepository<FootballSimulatorDbContext, StadiumType>
    {
        public StadiumTypeEFRepository(FootballSimulatorDbContext context) : base(context)
        {
        }
        protected override IQueryable<StadiumType> EntitySet => base.EntitySet.OrderBy(st => st.Name);
    }    
}
