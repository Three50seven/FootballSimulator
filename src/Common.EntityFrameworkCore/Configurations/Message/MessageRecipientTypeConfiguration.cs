using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class MessageRecipientTypeConfiguration : IEntityTypeConfiguration<MessageRecipientType>
    {
        public void Configure(EntityTypeBuilder<MessageRecipientType> builder)
        {
            builder.ConfigureLookupEntityProperties<MessageRecipientType, MessageRecipientTypeOption>(
                (typeEnum) => new MessageRecipientType(typeEnum));
        }
    }
}
