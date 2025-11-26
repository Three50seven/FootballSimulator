using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public static class MessageAddressBuilderExtensions
    {
        public static OwnedNavigationBuilder<TEntity, MessageAddress> ConfigureMessageAddress<TEntity>(
          this OwnedNavigationBuilder<TEntity, MessageAddress> messageAddrBuilder,
          string emailColumnName = null,
          string nameColumnName = null)
           where TEntity : class
        {
            var emailProperty = messageAddrBuilder.Property(ma => ma.Email).IsRequired().HasMaxLength(200);
            var nameProperty = messageAddrBuilder.Property(ma => ma.Name).IsRequired(false).HasMaxLength(200);

            if (!string.IsNullOrWhiteSpace(emailColumnName))
                emailProperty.HasColumnName(emailColumnName);

            if (!string.IsNullOrWhiteSpace(nameColumnName))
                nameProperty.HasColumnName(nameColumnName);

            return messageAddrBuilder;
        }
    }
}
