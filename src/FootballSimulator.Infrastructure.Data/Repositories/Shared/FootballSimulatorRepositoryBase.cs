using Common.Core;
using Common.Core.Domain;
using Common.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public abstract class FootballSimulatorRepositoryBase<TType, TKey> : FootballSimulatorRepositoryBase<TType, TKey, RepositoryIncludesDefaultOption>
        where TType : class, IEntity<TKey>
    {
        protected FootballSimulatorRepositoryBase(FootballSimulatorDbContext context)
            : base(context)
        {
        }
    }

    public abstract class FootballSimulatorRepositoryBase<TType, TKey, TIncludes> : EFRepositoryBase<FootballSimulatorDbContext, TType, TKey, TIncludes>
        where TType : class, IEntity<TKey>
        where TIncludes : struct, Enum
    {
        protected FootballSimulatorRepositoryBase(FootballSimulatorDbContext context)
            : base(context) { }
    }
}
