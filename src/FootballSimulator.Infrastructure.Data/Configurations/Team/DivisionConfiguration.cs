using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class DivisionConfiguration : IEntityTypeConfiguration<Division>
    {
        public void Configure(EntityTypeBuilder<Division> builder)
        {
            builder.ConfigureLookupEntityProperties(generateIdOnDatabase: true);
            builder.HasOne(d => d.Conference)
                   .WithMany(c => c.Divisions)
                   .HasForeignKey(d => d.ConferenceId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
