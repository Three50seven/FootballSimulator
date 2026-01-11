using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class SeasonWeekConfiguration : IEntityTypeConfiguration<SeasonWeek>
    {
        public void Configure(EntityTypeBuilder<SeasonWeek> builder)
        {
            builder.ConfigureDomainEntityProperties();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.HasOne(e => e.Season).WithMany(e => e.Weeks).HasForeignKey(e => e.SeasonId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
