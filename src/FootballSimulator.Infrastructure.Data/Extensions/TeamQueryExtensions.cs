using Common.EntityFrameworkCore;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public static class TeamQueryExtensions
    {
        public static IQueryable<Team> IncludeAll(this IQueryable<Team> query)
        {
            return query
                .Include(t => t.Stadium)
                    .ThenInclude(s => s.StadiumType)
                .Include(t => t.Stadium)
                    .ThenInclude(s => s.City)
                    .ThenInclude(c => c.State)
                    .ThenInclude(st => st.Country)
                .Include(t => t.Division)
                .Include(t => t.Division)
                    .ThenInclude(d => d.Conference)
                .IncludeChangeEvents()
                .AsSplitQuery();
        }

        public static IQueryable<Team> Include(this IQueryable<Team> query, TeamQueryIncludeOption includes)
        {
            return includes switch
            {
                TeamQueryIncludeOption.All => query.IncludeAll(),
                TeamQueryIncludeOption.None => query
                    .IncludeChangeEvents(),
                TeamQueryIncludeOption.Stadium => query
                    .Include(t => t.Stadium)
                        .ThenInclude(s => s.StadiumType)
                    .Include(t => t.Stadium)
                        .ThenInclude(s => s.City)
                        .ThenInclude(c => c.State)
                        .ThenInclude(st => st.Country)
                    .IncludeChangeEvents(),
                TeamQueryIncludeOption.Division => query
                    .Include(t => t.Division)
                    .Include(t => t.Division)
                        .ThenInclude(d => d.Conference)
                    .IncludeChangeEvents(),
                _ => query
            };
        }
    }
}
