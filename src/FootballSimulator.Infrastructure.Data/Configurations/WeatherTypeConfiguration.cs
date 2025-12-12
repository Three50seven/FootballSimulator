using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data.Configurations
{
    internal class WeatherTypeConfiguration : IEntityTypeConfiguration<WeatherType>
    {
        public void Configure(EntityTypeBuilder<WeatherType> builder)
        {
            builder.ConfigureLookupEntityProperties(generateIdOnDatabase: true);
        }
    }
}
