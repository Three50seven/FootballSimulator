using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public class EntityHistoryChangeConfiguration : IEntityTypeConfiguration<EntityHistoryChange>
    {
        public void Configure(EntityTypeBuilder<EntityHistoryChange> builder)
        {
            builder.OwnsOne(ehc => ehc.Property, propertyBuilder =>
            {
                propertyBuilder.Property(p => p.Name).IsRequired(true).HasMaxLength(200);
                propertyBuilder.Property(p => p.FriendlyName).IsRequired(true).HasMaxLength(200);
                propertyBuilder.Property(p => p.Type).IsRequired(true).HasMaxLength(500);
            });

            builder.OwnsOne(ehc => ehc.Change, changeBuilder =>
            {
                changeBuilder.Property(c => c.Description).HasColumnName("description").IsRequired(false);
                changeBuilder.Property(c => c.OldValue).HasColumnName("old_value").IsRequired(false);
                changeBuilder.Property(c => c.NewValue).HasColumnName("new_value").IsRequired(false);
            });
        }
    }
}
