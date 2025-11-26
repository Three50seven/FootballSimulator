using Microsoft.EntityFrameworkCore;
using Common.Core.Validation;
using System;

namespace Common.EntityFrameworkCore
{
    public static class ModelBuilderMessageExtensions
    {
        /// <summary>
        /// Applies entity configurations for standard <see cref="Common.Core.Domain.Message"/>. 
        /// Includes category, recipient, and attachment entities.
        /// </summary>
        /// <typeparam name="TCategoryEnumType">Custom enum to represent message categories</typeparam>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder AddMessageConfigurations<TCategoryEnumType>(this ModelBuilder modelBuilder)
            where TCategoryEnumType : Enum
        {
            Guard.IsNotNull(modelBuilder, nameof(modelBuilder));

            return modelBuilder.ApplyConfiguration(new MessageConfiguration())
                               .ApplyConfiguration(new MessageCategoryConfiguration<TCategoryEnumType>())
                               .ApplyConfiguration(new MessageRecipientConfiguration())
                               .ApplyConfiguration(new MessageRecipientTypeConfiguration())
                               .ApplyConfiguration(new MessageAttachmentConfiguration())
                               .ApplyConfiguration(new MessageReplyAddressConfiguration());
        }
    }
}
