using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class MessageReplyAddressConfiguration : IEntityTypeConfiguration<MessageReplyAddress>
    {
        public void Configure(EntityTypeBuilder<MessageReplyAddress> builder)
        {
            builder.OwnsOne(r => r.Address, addrBuilder => addrBuilder.ConfigureMessageAddress(emailColumnName: "email", nameColumnName: "name"));
            builder.HasOne(r => r.Message).WithMany(m => m.ReplyTos).HasForeignKey(r => r.MessageId);
        }
    }
}
