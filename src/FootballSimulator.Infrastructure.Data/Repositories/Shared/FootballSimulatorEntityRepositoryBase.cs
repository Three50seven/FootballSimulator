using Common.Core;
using Common.Core.Domain;
using Common.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public abstract class FootballSimulatorEntityRepositoryBase<TEntity> : EFDomainRepositoryBase<FootballSimulatorDbContext, TEntity>
        where TEntity : class, IDomainEntity, IHistoricalEntity
    {
        protected FootballSimulatorEntityRepositoryBase(FootballSimulatorDbContext dbContext)
            : base(dbContext)
        {
        }

        protected FootballSimulatorEntityRepositoryBase(FootballSimulatorDbContext dbContext, IEntityHistoryStore historyStore)
            : base(dbContext, historyStore)
        {
        }
    }

    public abstract class FootballSimulatorEntityRepositoryBase<TEntity, TIncludes> : EFDomainRepositoryBase<FootballSimulatorDbContext, TEntity, TIncludes>
        where TEntity : class, IDomainEntity, IHistoricalEntity
        where TIncludes : struct, Enum
    {
        protected FootballSimulatorEntityRepositoryBase(FootballSimulatorDbContext dbContext)
            : base(dbContext)
        {
        }
        protected FootballSimulatorEntityRepositoryBase(FootballSimulatorDbContext dbContext, IEntityHistoryStore historyStore)
            : base(dbContext, historyStore)
        {
        }
    }
}
