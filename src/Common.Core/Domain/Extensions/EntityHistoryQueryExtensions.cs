using System.Collections.Generic;
using System.Linq;

namespace Common.Core.Domain
{
    public static class EntityHistoryQueryExtensions
    {
        /// <summary>
        /// Gets the command events for the created event and the latest updated event into a <see cref="ChangeEvents"/> result.
        /// </summary>
        /// <param name="entityHistories"></param>
        /// <returns></returns>
        public static ChangeEvents SelectCreatedUpdatedEvents(this IQueryable<EntityHistory> entityHistories)
        {
            var created = entityHistories.OrderBy(x => x.Event.Date).Select(x => x.Event).FirstOrDefault();
            var updated = entityHistories.OrderByDescending(x => x.Event.Date).Select(x => x.Event).FirstOrDefault();

            return new ChangeEvents(created, updated);
        }

        /// <summary>
        /// Gets the command events for the created event and the latest updated event into a <see cref="ChangeEvents"/> result.
        /// </summary>
        /// <param name="entityHistories"></param>
        /// <returns></returns>
        public static ChangeEvents SelectCreatedUpdatedEvents(this IEnumerable<EntityHistory> entityHistories)
        {
            var created = entityHistories.OrderBy(x => x.Event.Date).Select(x => x.Event).FirstOrDefault();
            var updated = entityHistories.OrderByDescending(x => x.Event.Date).Select(x => x.Event).FirstOrDefault();

            return new ChangeEvents(created, updated);
        }
    }
}
