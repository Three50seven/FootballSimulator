using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;
using System;

namespace Common.EntityFrameworkCore
{
    public class MessageCategoryConfiguration<TEnumType> : IEntityTypeConfiguration<MessageCategory>
        where TEnumType : Enum
    {
        public void Configure(EntityTypeBuilder<MessageCategory> builder)
        {
            builder.ConfigureLookupEntityProperties<MessageCategory, TEnumType>(
                (categoryEnum) => new MessageCategory(categoryEnum));
        }
    }
}
