using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ConfigureLookupEntityProperties(generateIdOnDatabase: true);

            builder.Property(s => s.Abbreviation).HasMaxLength(50).IsRequired();
            builder.HasOne(x => x.Country).WithMany(x => x.States).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(s => s.Fips).HasMaxLength(50).IsRequired(false);
        }
    }
}
