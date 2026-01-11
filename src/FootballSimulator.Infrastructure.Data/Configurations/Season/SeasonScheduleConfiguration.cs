using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class SeasonScheduleConfiguration : IEntityTypeConfiguration<SeasonSchedule>
    {
        public void Configure(EntityTypeBuilder<SeasonSchedule> builder)
        {
            builder.ConfigureDomainEntityProperties();
            builder.HasOne(ss => ss.SeasonWeek)
                   .WithMany(sw => sw.SeasonSchedules)
                   .HasForeignKey(ss => ss.WeekId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ss => ss.HomeTeam)
                     .WithMany()
                     .HasForeignKey(ss => ss.HomeTeamId)
                     .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ss => ss.AwayTeam)
                        .WithMany()
                        .HasForeignKey(ss => ss.AwayTeamId)
                        .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
