using Microsoft.EntityFrameworkCore;
using Common.Core.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace Common.EntityFrameworkCore
{
    public static class EntityHistoryQueryExtensions
    {
        /// <summary>
        /// Gets the command events async for the created event and the latest updated event into a <see cref="ChangeEvents"/> result.
        /// This async version mirrors the extension <see cref="Common.Core.Domain.EntityHistoryQueryExtensions.SelectCreatedUpdatedEvents(IQueryable{EntityHistory})"/>
        /// but uses the async linq operations.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<ChangeEvents> SelectCreatedUpdatedEventsAsync(this IQueryable<EntityHistory> query)
        {
            if (query == null)
                return null;

            var created = await query.OrderBy(x => x.Event.Date).Select(x => x.Event).FirstOrDefaultAsync();
            var updated = await query.OrderByDescending(x => x.Event.Date).Select(x => x.Event).FirstOrDefaultAsync();

            return new ChangeEvents(created, updated);
        }
    }
}
