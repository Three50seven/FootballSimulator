using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(au => au.UserId)
                   .IsRequired(false);

            builder.ToTable("application_users");

            builder.HasOne(u => u.User)
                   .WithOne()                   
                   .HasForeignKey<ApplicationUser>(au => au.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
