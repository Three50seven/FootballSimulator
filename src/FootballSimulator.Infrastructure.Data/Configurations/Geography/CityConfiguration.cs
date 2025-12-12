using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ConfigureLookupEntityProperties(generateIdOnDatabase: true);
            builder.HasOne(c => c.State).WithMany(s => s.Cities).HasForeignKey(c => c.StateId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
