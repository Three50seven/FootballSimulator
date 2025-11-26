using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public static class MessageBuilderExtensions
    {
        /// <summary>
        /// Configure standard Message properties and establish FK relationship with <see cref="MessageCategory"/>.
        /// </summary>
        /// <typeparam name="T">Message entity type.</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static EntityTypeBuilder<T> ConfigureMessageProperties<T>(
            this EntityTypeBuilder<T> builder)
            where T : Message
        {
            builder.Property(x => x.Subject).IsRequired(false).HasMaxLength(1000);
            builder.OwnsOne(m => m.From, fromBuilder => fromBuilder.ConfigureMessageAddress());

            builder.Property(m => m.Content).IsRequired();
            builder.Property(m => m.Error).IsRequired(false);
            builder.Property(m => m.ProcessedDate).IsRequired(false);
            builder.Property(m => m.IsHtml).IsRequired(true).HasDefaultValue(false);

            builder.Ignore(m => m.HasError);
            builder.Ignore(m => m.ToAddresses);
            builder.Ignore(m => m.CcAddresses);
            builder.Ignore(m => m.BccAddresses);
            builder.Ignore(m => m.ToRecipients);
            builder.Ignore(m => m.ReplyToAddresses);

            builder.HasOne(m => m.Category).WithMany().HasForeignKey(x => x.CategoryId);

            return builder;
        }
    }
}
