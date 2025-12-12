using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    public class ClimateTypeWeatherTypeConfiguration : IEntityTypeConfiguration<ClimateTypeWeatherType>
    {
        public void Configure(EntityTypeBuilder<ClimateTypeWeatherType> builder)
        {
            builder.HasKey(ctwt => new { ctwt.ClimateTypeId, ctwt.WeatherTypeId });
            builder.HasOne(ctwt => ctwt.ClimateType)
                   .WithMany(ct => ct.ClimateTypeWeatherTypes)
                   .HasForeignKey(ctwt => ctwt.ClimateTypeId);
            builder.HasOne(ctwt => ctwt.WeatherType)
                     .WithMany(wt => wt.ClimateTypeWeatherTypes)
                     .HasForeignKey(ctwt => ctwt.WeatherTypeId);
        }
    }
}
