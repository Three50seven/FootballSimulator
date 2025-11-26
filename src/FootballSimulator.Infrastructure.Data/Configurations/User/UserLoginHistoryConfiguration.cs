using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    internal class UserLoginHistoryConfiguration : IEntityTypeConfiguration<UserLoginHistory>
    {
        public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
        {
            builder.Property(h => h.IpAddress).HasMaxLength(50).IsRequired(false);
            builder.OwnsOne(h => h.Event,
                eventBuilder => eventBuilder.ConfigureCommandEventProperties<UserLoginHistory, User>());
        }
    }
}
