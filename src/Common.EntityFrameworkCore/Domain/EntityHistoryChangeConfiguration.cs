using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public class EntityHistoryChangeConfiguration : IEntityTypeConfiguration<EntityHistoryChange>
    {
        public void Configure(EntityTypeBuilder<EntityHistoryChange> builder)
        {
            builder.OwnsOne((EntityHistoryChange ehc) => ehc.Property, delegate (OwnedNavigationBuilder<EntityHistoryChange, EntityProperty> propertyBuilder)
            {
                propertyBuilder.Property((EntityProperty p) => p.Name).IsRequired().HasMaxLength(200);
                propertyBuilder.Property((EntityProperty p) => p.FriendlyName).IsRequired().HasMaxLength(200);
                propertyBuilder.Property((EntityProperty p) => p.Type).IsRequired().HasMaxLength(500);
            });
            builder.OwnsOne((EntityHistoryChange ehc) => ehc.Change, delegate (OwnedNavigationBuilder<EntityHistoryChange, EntityChange> changeBuilder)
            {
                changeBuilder.Property((EntityChange c) => c.Description).HasColumnName("description").IsRequired(required: false);
                changeBuilder.Property((EntityChange c) => c.OldValue).HasColumnName("old_value").IsRequired(required: false);
                changeBuilder.Property((EntityChange c) => c.NewValue).HasColumnName("new_value").IsRequired(required: false);
            });
        }
    }
}
