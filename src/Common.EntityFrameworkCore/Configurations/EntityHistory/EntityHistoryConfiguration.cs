using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public class EntityHistoryConfiguration<TUser> : IEntityTypeConfiguration<EntityHistory> 
        where TUser : class, IUser
    {
        public void Configure(EntityTypeBuilder<EntityHistory> builder)
        {
            builder.HasOne(eh => eh.Type).WithMany().HasForeignKey(eh => eh.TypeId);
            builder.OwnsOne(eh => eh.Event, eventBuilder => eventBuilder.ConfigureCommandEventProperties<EntityHistory, TUser>());
            builder.HasMany(eh => eh.Changes).WithOne(c => c.EntityHistory).HasForeignKey(c => c.EntityHistoryId);
        }
    }
}
