using Common.Core.Domain;
using Common.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public abstract class FootballSimulatorLookupRepositoryBase<TLookupEntity> : EFLookupRepository<FootballSimulatorDbContext, TLookupEntity>
        where TLookupEntity : class, ILookupEntity
    {
        protected FootballSimulatorLookupRepositoryBase(FootballSimulatorDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
