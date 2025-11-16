using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public class EntityHistoryConfiguration<TUser> : IEntityTypeConfiguration<EntityHistory> where TUser : class, IUser
    {
        public void Configure(EntityTypeBuilder<EntityHistory> builder)
        {
            builder.HasOne((EntityHistory eh) => eh.Type).WithMany().HasForeignKey((EntityHistory eh) => eh.TypeId);
            builder.OwnsOne((EntityHistory eh) => eh.Event, delegate (OwnedNavigationBuilder<EntityHistory, UserCommandEvent> eventBuilder)
            {
                eventBuilder.ConfigureCommandEventProperties<EntityHistory, TUser>();
            });
            builder.HasMany((EntityHistory eh) => eh.Changes).WithOne((EntityHistoryChange c) => c.EntityHistory).HasForeignKey((EntityHistoryChange c) => c.EntityHistoryId);
        }
    }
}
