using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.ConfigureDomainEntityProperties();
            builder.HasOne(s => s.Type)
                   .WithMany()
                   .HasForeignKey(s => s.TypeId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.Property(s => s.Name).HasMaxLength(100);
        }
    }
}