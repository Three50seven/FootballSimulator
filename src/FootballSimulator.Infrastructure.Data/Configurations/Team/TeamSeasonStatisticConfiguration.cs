using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class TeamSeasonStatisticConfiguration : IEntityTypeConfiguration<TeamSeasonStatistic>
    {
        public void Configure(EntityTypeBuilder<TeamSeasonStatistic> builder)
        {
            builder.ConfigureDomainEntityProperties();
            builder.HasOne(tss => tss.Team)
                   .WithMany(t => t.TeamSeasonStatistics)
                   .HasForeignKey(tss => tss.TeamId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(tss => tss.Season)
                     .WithMany()
                     .HasForeignKey(tss => tss.SeasonId)
                     .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(tss => tss.PostSeason)
                        .WithMany()
                        .HasForeignKey(tss => tss.PostSeasonId)
                        .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(tss => tss.QualificationType)
                            .WithMany()
                            .HasForeignKey(tss => tss.QualificationTypeId)
                            .OnDelete(DeleteBehavior.Restrict);

            var decimalProperties = typeof(TeamSeasonStatistic)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(decimal));

            foreach (var property in decimalProperties)
            {
                builder.Property(property.Name)
                       .HasColumnType("decimal(6, 5)")
                       .IsRequired();
            }
        }
    }
}
