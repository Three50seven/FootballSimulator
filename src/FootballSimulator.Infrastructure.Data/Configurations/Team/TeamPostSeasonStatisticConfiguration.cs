using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class TeamPostSeasonStatisticConfiguration : IEntityTypeConfiguration<TeamPostSeasonStatistic>
    {
        public void Configure(EntityTypeBuilder<TeamPostSeasonStatistic> builder)
        {
            builder.ConfigureDomainEntityProperties();
            builder.HasOne(e => e.Team)
                .WithMany(t => t.TeamPostSeasonStatistics)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Season)
                .WithMany()
                .HasForeignKey(e => e.SeasonId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.PreviousRegularSeason)
                .WithMany()
                .HasForeignKey(e => e.PreviousRegularSesasonId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.PreviousPostSeason)
                .WithMany()
                .HasForeignKey(e => e.PreviousPostSeasonId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.StatisticGenerationMethodType)
                .WithMany()
                .HasForeignKey(e => e.StatisticGenerationMethodTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            var decimalProperties = typeof(TeamPostSeasonStatistic)
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
