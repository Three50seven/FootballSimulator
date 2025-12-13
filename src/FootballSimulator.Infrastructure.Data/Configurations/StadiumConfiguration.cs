using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class StadiumConfiguration : IEntityTypeConfiguration<Stadium>
    {
        public void Configure(EntityTypeBuilder<Stadium> builder)
        {
            builder.ConfigureFSDataEntityProperties();
            builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
            builder.HasOne(s => s.City).WithMany().HasForeignKey(s => s.CityId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.StadiumType).WithMany().HasForeignKey(s => s.StadiumTypeId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.ClimateType).WithMany().HasForeignKey(s => s.ClimateTypeId).OnDelete(DeleteBehavior.Restrict);                
        }
    }
}
