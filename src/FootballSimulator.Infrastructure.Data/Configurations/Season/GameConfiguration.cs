using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ConfigureDomainEntityProperties();            
            builder.Property(g => g.WindDirection).HasMaxLength(50);
            builder.HasOne(g => g.SeasonSchedule)
                   .WithMany(ss => ss.Games)
                   .HasForeignKey(g => g.SeasonScheduleId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(g => g.Stadium)
                   .WithMany()
                   .HasForeignKey(g => g.StadiumId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(g => g.WeatherType)
                     .WithMany()
                     .HasForeignKey(g => g.WeatherTypeId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
