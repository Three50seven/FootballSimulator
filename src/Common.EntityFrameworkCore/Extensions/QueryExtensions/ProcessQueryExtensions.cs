using Microsoft.EntityFrameworkCore;
using Common.Core.Domain;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    public static class ProcessQueryExtensions
    {
        /// <summary>
        /// Includes Type, Parameters, Results values, and Retries on the <see cref="Process"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<Process> IncludeAllRelations(this IQueryable<Process> query)
        {
            return query?.Include(p => p.Type)
                         .Include(p => p.ScheduleType)
                         .Include(p => p.Parameters)
                         .Include(p => p.Results)
                         .Include(p => p.Retries);
        }
    }
}
