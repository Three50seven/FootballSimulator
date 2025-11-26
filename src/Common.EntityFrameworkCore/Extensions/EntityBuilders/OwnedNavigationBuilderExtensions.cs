using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public static class OwnedNavigationBuilderExtensions
    {
        public const string CreatedDateColumnName = "created_date";
        public const string CreatedUserIdColumnName = "created_user_id";
        public const string UpdatedDateColumnName = "last_updated_date";
        public const string UpdatedUserIdColumnName = "last_updated_user_id";

        /// <summary>
        /// Set standard configuration for <see cref="FileDetail"/> as an owned type.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static OwnedNavigationBuilder<TEntity, FileDetail> ConfigureFileDetailProperties<TEntity>(
            this OwnedNavigationBuilder<TEntity, FileDetail> builder)
            where TEntity : class
        {
            builder.Property(f => f.FileName).HasMaxLength(500).IsRequired();
            builder.Property(f => f.ContentType).HasMaxLength(100).IsRequired();
            builder.Property(f => f.Extension).HasMaxLength(50).IsRequired();
            builder.Property(f => f.ContentLength).IsRequired();

            return builder;
        }

        /// <summary>
        /// Default property configuration for <see cref="Name"/> owned type.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="includeDefaultPrefix">
        /// Whether to include the defaulted prefix that EF will apply (propname_first_name, etc.). 
        /// If false, first_name and last_name will be used. Defaults to true.</param>
        /// <param name="required">Whether both first and last name fields are required. Defaults to true.</param>
        /// <returns></returns>
        public static OwnedNavigationBuilder<TEntity, Name> ConfigureNameProperties<TEntity>(
            this OwnedNavigationBuilder<TEntity, Name> builder, bool includeDefaultPrefix = true, bool required = true)
            where TEntity : class
        {
            var firstNameBuilder = builder.Property(n => n.FirstName).IsRequired(required).HasMaxLength(200);
            var lastNameBuilder = builder.Property(n => n.LastName).IsRequired(required).HasMaxLength(200);

            if (!includeDefaultPrefix)
            {
                firstNameBuilder.HasColumnName("first_name");
                lastNameBuilder.HasColumnName("last_name");
            }

            return builder;
        }

        /// <summary>
        /// Default property configuration for <see cref="CommandDate"/> owned type.
        /// Will set <see cref="CommandDate{T}.Date"/> to have a default SQL value of GETUTCDATE();
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="includeDefaultPrefix">Whether to include the defaulted prefix that EF will apply (propname_user_id, etc.). 
        /// If false, user_id and date will be used. Defaults to true.</param>
        /// <returns></returns>
        public static OwnedNavigationBuilder<TEntity, CommandDate> ConfigureCommandDateProperties<TEntity>(
            this OwnedNavigationBuilder<TEntity, CommandDate> builder, bool includeDefaultPrefix = true)
            where TEntity : class
        {
            var userIdBuilder = builder.Property(e => e.UserId).IsRequired();
            var dateBuilder = builder.Property(e => e.Date).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            if (!includeDefaultPrefix)
            {
                userIdBuilder.HasColumnName("user_id");
                dateBuilder.HasColumnName("date");
            }

            return builder;
        }
    }
}
