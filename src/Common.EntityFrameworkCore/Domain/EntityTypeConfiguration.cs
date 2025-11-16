using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public class EntityTypeConfiguration<TEnumType> : IEntityTypeConfiguration<EntityType> where TEnumType : Enum
    {
        public void Configure(EntityTypeBuilder<EntityType> builder)
        {
            builder.ConfigureLookupEntityProperties((TEnumType type) => new EntityType(type));
        }
    }
}
