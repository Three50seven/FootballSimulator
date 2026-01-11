using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class DivisionRotationScheduleConfiguration : IEntityTypeConfiguration<DivisionRotationSchedule>
    {
        public void Configure(EntityTypeBuilder<DivisionRotationSchedule> builder)
        {
            builder.ConfigureDomainEntityProperties();
            builder.Property(e => e.YearCycle).IsRequired();
            builder.HasOne(e => e.GameType)
                .WithMany()
                .HasForeignKey(e => e.GameTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Division)
                .WithMany()
                .HasForeignKey(e => e.DivisionId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.OponentDivision)
                .WithMany()
                .HasForeignKey(e => e.OponentDivisionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
