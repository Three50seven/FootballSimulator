using Microsoft.EntityFrameworkCore;
using Common.Core.Domain;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    public static class EntityChangeEventStorableQueryExtensions
    {
        /// <summary>
        /// Include associated User entity on ChangeEvents (added/updated).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<T> IncludeChangeEvents<T>(this IQueryable<T> query)
           where T : class, IEntityChangeEventStorable
        {
            return query.Include(a => a.ChangeEvents.Created.User)
                        .Include(a => a.ChangeEvents.Updated.User);
        }
    }
}
