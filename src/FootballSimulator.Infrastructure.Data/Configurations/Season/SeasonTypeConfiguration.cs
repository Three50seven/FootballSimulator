using Common.EntityFrameworkCore;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class SeasonTypeConfiguration : IEntityTypeConfiguration<SeasonType>
    {
        public void Configure(EntityTypeBuilder<SeasonType> builder)
        {
            builder.ConfigureLookupEntityProperties<SeasonType, SeasonTypeOption>(type => new SeasonType(type));
        }
    }
}
