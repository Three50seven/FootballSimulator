using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class ConferenceConfiguration : IEntityTypeConfiguration<Conference>
    {
        public void Configure(EntityTypeBuilder<Conference> builder)
        {
            builder.ConfigureLookupEntityProperties(generateIdOnDatabase: true);
            builder.Property(c => c.Abbreviation)
                .HasMaxLength(10);
        }
    }
}
