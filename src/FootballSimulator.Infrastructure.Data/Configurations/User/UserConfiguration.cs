using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ConfigureDomainEntityProperties();

            builder.Property(u => u.UserName).HasMaxLength(100);
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.ApplicationUserGuid).HasMaxLength(450);

            builder.OwnsOne(u => u.Name, nameBuilder =>
            {
                nameBuilder.Property(n => n.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);
                nameBuilder.Property(n => n.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(u => u.ApplicationUserGuid)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
