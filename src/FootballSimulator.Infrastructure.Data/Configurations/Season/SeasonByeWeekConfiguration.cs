using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class SeasonByeWeekConfiguration : IEntityTypeConfiguration<SeasonByeWeek>
    {
        public void Configure(EntityTypeBuilder<SeasonByeWeek> builder)
        {
            builder.ConfigureDomainEntityProperties();
            builder.HasOne(sbw => sbw.Team)
                   .WithMany(t => t.SeasonByeWeeks)
                   .HasForeignKey(sbw => sbw.TeamId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sbw => sbw.SeasonWeek)
                     .WithMany(sw => sw.SeasonByeWeeks)
                     .HasForeignKey(sbw => sbw.SeasonWeekId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
