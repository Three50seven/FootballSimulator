using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class TeamRatingConfiguration : IEntityTypeConfiguration<TeamRating>
    {
        public void Configure(EntityTypeBuilder<TeamRating> builder)
        {
            builder.ConfigureDomainEntityProperties();
            builder.HasOne(tr => tr.Team)
                   .WithMany(t => t.TeamRatings)
                   .HasForeignKey(tr => tr.TeamId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(tr => tr.Season)
                     .WithMany()
                     .HasForeignKey(tr => tr.SeasonId)
                     .OnDelete(DeleteBehavior.Cascade);

            var decimalProperties = typeof(TeamRating)
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
