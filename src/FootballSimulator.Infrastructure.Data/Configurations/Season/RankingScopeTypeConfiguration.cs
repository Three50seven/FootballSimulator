using Common.EntityFrameworkCore;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class RankingScopeTypeConfiguration : IEntityTypeConfiguration<RankingScopeType>
    {
        public void Configure(EntityTypeBuilder<RankingScopeType> builder)
        {
            builder.ConfigureLookupEntityProperties<RankingScopeType, RankingScopeTypeOption>(type => new RankingScopeType(type));
        }
    }
}