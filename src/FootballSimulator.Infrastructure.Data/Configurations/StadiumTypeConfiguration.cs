using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class StadiumTypeConfiguration : IEntityTypeConfiguration<StadiumType>
    {
        public void Configure(EntityTypeBuilder<StadiumType> builder)
        {
            builder.ConfigureLookupEntityProperties(generateIdOnDatabase: true);
        }
    }
}
