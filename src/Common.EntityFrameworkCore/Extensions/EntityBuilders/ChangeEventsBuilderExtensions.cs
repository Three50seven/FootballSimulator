using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public static class ChangeEventsBuilderExtensions
    {
        /// <summary>
        /// Configure Created and Updated properties for the <see cref="ChangeEvents"/> property on a given entity.
        /// Uses "created" and "updated" prefixes. Sets User FK relationship on both. 
        /// Calls <see cref="UserCommandEventBuilderExtensions.ConfigureCommandEventProperties{TEntity, TUser}(OwnedNavigationBuilder{TEntity, UserCommandEvent}, string)"/>
        /// for both Created and Updated.
        /// </summary>
        /// <typeparam name="TEntity">Any entity type that has <see cref="ChangeEvents"/> property.</typeparam>
        /// <typeparam name="TUser">User type for the executing application.</typeparam>
        /// <param name="eventsBuilder"></param>
        /// <returns></returns>
        public static OwnedNavigationBuilder<TEntity, ChangeEvents> ConfigureChangeEventsProperties<TEntity, TUser>(
           this OwnedNavigationBuilder<TEntity, ChangeEvents> eventsBuilder)
            where TEntity : class
            where TUser : class, IUser
        {
            eventsBuilder.OwnsOne(ce => ce.Created, createBuilder =>
            {
                createBuilder.ConfigureCommandEventProperties<ChangeEvents, TUser>("created");
            });

            eventsBuilder.OwnsOne(ce => ce.Updated, updateBuilder =>
            {
                updateBuilder.ConfigureCommandEventProperties<ChangeEvents, TUser>("updated");
            });

            return eventsBuilder;
        }
    }
}
