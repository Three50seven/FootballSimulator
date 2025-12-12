using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class ClimateTypeConfiguration : IEntityTypeConfiguration<ClimateType>
    {
        public void Configure(EntityTypeBuilder<ClimateType> builder)
        {
            builder.ConfigureLookupEntityProperties(generateIdOnDatabase: true);

            builder.HasMany(ct => ct.WeatherTypes).WithMany(wt => wt.ClimateTypes)
                   .UsingEntity<ClimateTypeWeatherType>(
                        j => j
                            .HasOne(ctwt => ctwt.WeatherType)
                            .WithMany(wt => wt.ClimateTypeWeatherTypes)
                            .HasForeignKey(ctwt => ctwt.WeatherTypeId),
                        j => j
                            .HasOne(ctwt => ctwt.ClimateType)
                            .WithMany(ct => ct.ClimateTypeWeatherTypes)
                            .HasForeignKey(ctwt => ctwt.ClimateTypeId),
                        j =>
                        {
                            j.HasKey(t => new { t.ClimateTypeId, t.WeatherTypeId });
                            j.ToTable("climate_type_weather_types");
                        });
        }
    }
}
