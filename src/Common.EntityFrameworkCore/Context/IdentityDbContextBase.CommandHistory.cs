using Common.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Common.EntityFrameworkCore
{
    public abstract partial class IdentityDbContextBase<TUser, TKey> :
        IdentityDbContext<TUser, IdentityRole<TKey>, TKey>,
        IContextHistorical
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {

        private readonly IList<EntityHistory> _commandsHistory = new List<EntityHistory>();

        /// <summary>
        /// Adds the <paramref name="command"/> object to the list of current <see cref="EntityHistory"/> commands performed on the context.
        /// </summary>
        /// <param name="command">The history record representing the command to be stored.</param>
        /// <param name="onlyUnique">Whether only unique commands are allowed. If true and the same <see cref="EntityHistory.EntityGuid"/> and <see cref="EntityHistory.TypeId"/> items already exists, it will be overwritten.</param>
        public virtual void AddToHistory(EntityHistory command, bool onlyUnique = true)
        {
            if (onlyUnique)
            {
                // remove any other history item that has been added that matches the entity and the type 
                // (to help not have multiple histories for updating an entity during one save event)
                var existingEvent = _commandsHistory.Where(x => x.EntityGuid == command.EntityGuid && x.TypeId == command.TypeId).FirstOrDefault();
                if (existingEvent != null)
                    _commandsHistory.Remove(existingEvent);
            }

            _commandsHistory.Add(command);
        }

    }
}
