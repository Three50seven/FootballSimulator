using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public static class UserCommandEventBuilderExtensions
    {
        public static string DefaultUserIdColumn = "user_id";
        public static string DefaultDateColumn = "date";

        /// <summary>
        /// Configures standard settings and User relationship for a command event.
        /// Columns will be set as "user_id" and "date" with optional prefix.
        /// User will be established with FK relationship and date will default to current UTC time.
        /// </summary>
        /// <typeparam name="TEntity">Entity class type that has a <see cref="UserCommandEvent"/> property.</typeparam>
        /// <typeparam name="TUser">The user type for the running application.</typeparam>
        /// <param name="eventBuilder"></param>
        /// <param name="prefix">Optional prefix to apply to the database column names. No prefix by default.</param>
        public static OwnedNavigationBuilder<TEntity, UserCommandEvent> ConfigureCommandEventProperties<TEntity, TUser>(
            this OwnedNavigationBuilder<TEntity, UserCommandEvent> eventBuilder,
            string? prefix = null)
           where TEntity : class
           where TUser : class, IUser
        {
            string userIdColumn = DefaultUserIdColumn;
            string dateColumn = DefaultDateColumn;

            if (!string.IsNullOrWhiteSpace(prefix))
            {
                userIdColumn = $"{prefix}_{userIdColumn}";
                dateColumn = $"{prefix}_{dateColumn}";
            }

            eventBuilder.Property(c => c.UserId).HasColumnName(userIdColumn);
            eventBuilder.Property(c => c.Date).HasDefaultValueSql("GETUTCDATE()").HasColumnName(dateColumn);

            eventBuilder.HasOne(c => (TUser?)c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);

            return eventBuilder;
        }

        /// <summary>
        /// Configures standard settings and User relationship for an optional command event.
        /// Columns will be set as "user_id" and "date" with optional prefix.
        /// User will be established with FK relationship and date will default null.
        /// </summary>
        /// <typeparam name="TEntity">Entity class type that has a <see cref="UserCommandEvent"/> property.</typeparam>
        /// <typeparam name="TUser">The user type for the running application.</typeparam>
        /// <param name="eventBuilder"></param>
        /// <param name="prefix">Optional prefix to apply to the database column names. No prefix by default.</param>
        public static OwnedNavigationBuilder<TEntity, UserCommandEventOptional> ConfigureCommandEventProperties<TEntity, TUser>(
                    this OwnedNavigationBuilder<TEntity, UserCommandEventOptional> eventBuilder, string? prefix = null)
                    where TEntity : class
                    where TUser : class, IUser
        {
            string userIdColumn = DefaultUserIdColumn;
            string dateColumn = DefaultDateColumn;

            if (!string.IsNullOrWhiteSpace(prefix))
            {
                userIdColumn = $"{prefix}_{userIdColumn}";
                dateColumn = $"{prefix}_{dateColumn}";
            }

            eventBuilder.Property(c => c.UserId).HasColumnName(userIdColumn);
            eventBuilder.Property(c => c.Date).HasColumnName(dateColumn);

            eventBuilder.HasOne(c => (TUser?)c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);

            return eventBuilder;
        }
    }
}
