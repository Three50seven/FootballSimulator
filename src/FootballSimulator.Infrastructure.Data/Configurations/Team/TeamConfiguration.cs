using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ConfigureFSDataEntityProperties();
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(t => t.Mascot)
                .HasMaxLength(200);
            builder.HasOne(t => t.Division)
                .WithMany(d => d.Teams)
                .HasForeignKey(t => t.DivisionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            builder.HasOne(t => t.Stadium)
                .WithMany(s => s.Teams)
                .HasForeignKey(t => t.StadiumId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
