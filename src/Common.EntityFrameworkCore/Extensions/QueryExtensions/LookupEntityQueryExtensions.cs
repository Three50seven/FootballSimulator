using Microsoft.EntityFrameworkCore;
using Common.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.EntityFrameworkCore
{
    public static class LookupEntityQueryExtensions
    {
        /// <summary>
        /// Get list of lookup entity items as <see cref="SelectItem"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="loadFullDataFirst"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<SelectItem>> ToSelectionsAsync<T>(this IQueryable<T> query, bool loadFullDataFirst = false)
            where T : ILookupEntity
        {
            if (loadFullDataFirst)
            {
                var data = await query.ToListAsync();
                return data.Select(e => e.ToSelectItem());
            }
            else
            {
                return await query.Select(e => new SelectItem(e.Id, e.Name)).ToListAsync();
            }
        }

        /// <summary>
        /// Get list of lookup entity items as <see cref="SelectItem"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="loadFullDataFirst"></param>
        /// <returns></returns>
        public static IEnumerable<SelectItem> ToSelections<T>(this IQueryable<T> query, bool loadFullDataFirst = false)
            where T : ILookupEntity
        {
            if (loadFullDataFirst)
                return query.ToList().Select(e => e.ToSelectItem());
            else
                return query.Select(e => new SelectItem(e.Id, e.Name)).ToList();
        }
    }
}
