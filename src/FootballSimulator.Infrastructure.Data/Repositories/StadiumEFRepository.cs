using Common.Core;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public class StadiumEFRepository : FootballSimulatorEntityRepositoryBase<Stadium>, IStadiumRepository
    {
        public StadiumEFRepository(FootballSimulatorDbContext dbContext, IEntityHistoryStore historyStore) : base(dbContext, historyStore)
        {
        }

        protected override IQueryable<Stadium> EntitySet => base.EntitySet
                                                            .Include(s => s.City)
                                                            .Include(s => s.StadiumType)
                                                            .Include(s => s.ClimateType)
                                                            .OrderBy(s => s.Name);
    }
}
