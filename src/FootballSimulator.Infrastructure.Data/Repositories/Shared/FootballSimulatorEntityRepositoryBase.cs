using Common.Core;
using Common.Core.Domain;
using Common.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public abstract class FootballSimulatorEntityRepositoryBase<TEntity> : EFStatelessDomainRepositoryBase<FootballSimulatorDbContext, TEntity>
        where TEntity : class, IDomainEntity, IHistoricalEntity
    {
        protected FootballSimulatorEntityRepositoryBase(IDbContextFactory<FootballSimulatorDbContext> factory)
            : base(factory)
        {
        }

        protected FootballSimulatorEntityRepositoryBase(IDbContextFactory<FootballSimulatorDbContext> factory, IEntityHistoryStore historyStore)
            : base(factory, historyStore)
        {
        }
    }

    public abstract class FootballSimulatorEntityRepositoryBase<TEntity, TIncludes> : EFStatelessDomainRepositoryBase<FootballSimulatorDbContext, TEntity, TIncludes>
        where TEntity : class, IDomainEntity, IHistoricalEntity
        where TIncludes : struct, Enum
    {
        protected FootballSimulatorEntityRepositoryBase(IDbContextFactory<FootballSimulatorDbContext> factory)
            : base(factory)
        {
        }
        protected FootballSimulatorEntityRepositoryBase(IDbContextFactory<FootballSimulatorDbContext> factory, IEntityHistoryStore historyStore)
            : base(factory, historyStore)
        {
        }
    }
}
