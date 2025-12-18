using Common.EntityFrameworkCore;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public static class StadiumQueryExtensions
    {
        public static IQueryable<Stadium> IncludeAll(this IQueryable<Stadium> query)
        {
            return query
                .Include(s => s.StadiumType)
                .Include(s => s.City)
                    .ThenInclude(c => c.State)
                    .ThenInclude(st => st.Country)
                .Include(s => s.Teams)
                .Include(s => s.ClimateType)                
                .IncludeChangeEvents()
                .AsSplitQuery();
        }

        public static IQueryable<Stadium> Include(this IQueryable<Stadium> query, StadiumQueryIncludeOption includes)
        {
            return includes switch
            {
                StadiumQueryIncludeOption.All => query.IncludeAll(),
                StadiumQueryIncludeOption.None => query
                    .IncludeChangeEvents(),
                StadiumQueryIncludeOption.Team => query
                    .Include(s => s.Teams)
                    .IncludeChangeEvents(),
                StadiumQueryIncludeOption.Geography => query
                    .Include(s => s.City)
                        .ThenInclude(c => c.State)
                        .ThenInclude(st => st.Country)
                        .IncludeChangeEvents(),
                _ => query
            };           
        }
    }
}
