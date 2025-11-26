using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
    {
        public void Configure(EntityTypeBuilder<MessageAttachment> builder)
        {
            builder.HasKey(ma => new { ma.MessageId, ma.DocumentId });
            builder.HasOne(ma => ma.Message).WithMany(m => m.Attachments).HasForeignKey(ma => ma.MessageId);
            builder.HasOne(ma => ma.Document).WithMany().HasForeignKey(ma => ma.DocumentId);
        }
    }
}
