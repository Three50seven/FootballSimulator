using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class ClimateTypeConfiguration : IEntityTypeConfiguration<ClimateType>
    {
        public void Configure(EntityTypeBuilder<ClimateType> builder)
        {
            builder.ConfigureLookupEntityProperties(generateIdOnDatabase: true);
        }
    }
}
