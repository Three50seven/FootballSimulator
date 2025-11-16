using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public static class UserCommandEventBuilderExtensions
    {
        public static string DefaultUserIdColumn = "user_id";

        public static string DefaultDateColumn = "date";

        //
        // Summary:
        //     Configures standard settings and User relationship for a command event. Columns
        //     will be set as "user_id" and "date" with optional prefix. User will be established
        //     with FK relationship and date will default to current UTC time.
        //
        // Parameters:
        //   eventBuilder:
        //
        //   prefix:
        //     Optional prefix to apply to the database column names. No prefix by default.
        //
        //
        // Type parameters:
        //   TEntity:
        //     Entity class type that has a Common.Core.Domain.UserCommandEvent property.
        //
        //
        //   TUser:
        //     The user type for the running application.
        public static OwnedNavigationBuilder<TEntity, UserCommandEvent> ConfigureCommandEventProperties<TEntity, TUser>(this OwnedNavigationBuilder<TEntity, UserCommandEvent> eventBuilder, string? prefix = null) where TEntity : class where TUser : class, IUser
        {
            string text = DefaultUserIdColumn;
            string text2 = DefaultDateColumn;
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                text = prefix + "_" + text;
                text2 = prefix + "_" + text2;
            }

            eventBuilder.Property((UserCommandEvent c) => c.UserId).HasColumnName(text);
            eventBuilder.Property((UserCommandEvent c) => c.Date).HasDefaultValueSql("GETUTCDATE()").HasColumnName(text2);
            eventBuilder.HasOne((UserCommandEvent c) => ((TUser?)c.User)!)
                .WithMany()
                .HasForeignKey((UserCommandEvent c) => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            return eventBuilder;
        }

        //
        // Summary:
        //     Configures standard settings and User relationship for an optional command event.
        //     Columns will be set as "user_id" and "date" with optional prefix. User will be
        //     established with FK relationship and date will default null.
        //
        // Parameters:
        //   eventBuilder:
        //
        //   prefix:
        //     Optional prefix to apply to the database column names. No prefix by default.
        //
        //
        // Type parameters:
        //   TEntity:
        //     Entity class type that has a Common.Core.Domain.UserCommandEvent property.
        //
        //
        //   TUser:
        //     The user type for the running application.
        public static OwnedNavigationBuilder<TEntity, UserCommandEventOptional> ConfigureCommandEventProperties<TEntity, TUser>(this OwnedNavigationBuilder<TEntity, UserCommandEventOptional> eventBuilder, string? prefix = null) where TEntity : class where TUser : class, IUser
        {
            string text = DefaultUserIdColumn;
            string text2 = DefaultDateColumn;
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                text = prefix + "_" + text;
                text2 = prefix + "_" + text2;
            }

            eventBuilder.Property((UserCommandEventOptional c) => c.UserId).HasColumnName(text);
            eventBuilder.Property((UserCommandEventOptional c) => c.Date).HasColumnName(text2);            
            eventBuilder.HasOne((UserCommandEventOptional c) => ((TUser?)c.User)!)
                .WithMany()
                .HasForeignKey((UserCommandEventOptional c) => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            return eventBuilder;
        }
    }
}
