using Common.EntityFrameworkCore;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class StatisticGenerationMethodTypeConfiguration : IEntityTypeConfiguration<StatisticGenerationMethodType>
    {
        public void Configure(EntityTypeBuilder<StatisticGenerationMethodType> builder)
        {
            builder.ConfigureLookupEntityProperties<StatisticGenerationMethodType, StatisticGenerationMethodTypeOption>(type => new StatisticGenerationMethodType(type));
        }
    }
}
