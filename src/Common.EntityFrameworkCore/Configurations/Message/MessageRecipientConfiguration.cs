using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class MessageRecipientConfiguration : IEntityTypeConfiguration<MessageRecipient>
    {
        public void Configure(EntityTypeBuilder<MessageRecipient> builder)
        {
            builder.OwnsOne(r => r.Address, addrBuilder => addrBuilder.ConfigureMessageAddress(emailColumnName: "email", nameColumnName: "name"));
            builder.HasOne(r => r.Type).WithMany().HasForeignKey(r => r.TypeId);
            builder.HasOne(r => r.Message).WithMany(m => m.Recipients).HasForeignKey(r => r.MessageId);

            builder.Ignore(r => r.TypeOption);
        }
    }
}
