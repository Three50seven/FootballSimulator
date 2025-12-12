using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public class StateEFRepository : EFLookupRepository<FootballSimulatorDbContext, State>
    {
        public StateEFRepository(FootballSimulatorDbContext context) : base(context)
        {
        }

        protected override IQueryable<State> EntitySet => base.EntitySet.Include(s => s.Cities);
    }
}
