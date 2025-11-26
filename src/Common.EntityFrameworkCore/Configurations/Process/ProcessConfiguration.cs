using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class ProcessConfiguration<TUser> : IEntityTypeConfiguration<Process>
        where TUser : class, IUser
    {
        public void Configure(EntityTypeBuilder<Process> builder)
        {
            builder.ConfigureDomainEntityProperties();

            builder.HasOne(p => p.ScheduleType).WithMany().HasForeignKey(p => p.ScheduleTypeId);
            builder.HasOne(p => p.Type).WithMany().HasForeignKey(p => p.TypeId);
            builder.Property(p => p.JobId).IsRequired(false).HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired(false);

            builder.OwnsOne(p => p.Created, createdEventBuilder => createdEventBuilder.ConfigureCommandEventProperties<Process, TUser>("created"));

            builder.OwnsOne(p => p.Started, startedEventBuilder =>
            {
                startedEventBuilder.Property(se => se.Date).IsRequired(false);
                startedEventBuilder.HasOne(se => (TUser)se.User).WithMany().HasForeignKey(se => se.UserId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.OwnsOne(p => p.Viewed, viewedEventBuilder =>
            {
                viewedEventBuilder.Property(se => se.Date).IsRequired(false);
                viewedEventBuilder.HasOne(se => (TUser)se.User).WithMany().HasForeignKey(se => se.UserId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
