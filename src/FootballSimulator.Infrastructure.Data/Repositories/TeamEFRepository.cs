using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.DTOs;
using FootballSimulator.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public class TeamEFRepository : FootballSimulatorEntityRepositoryBase<Team, TeamQueryIncludeOption>, ITeamRepository
    {
        public TeamEFRepository(IDbContextFactory<FootballSimulatorDbContext> factory) : base(factory)
        {
        }
        protected override IQueryable<Team> BuildQueryable(FootballSimulatorDbContext db, TeamQueryIncludeOption includes)
        {
            var query = base.BuildQueryable(db, includes);
            query = query.Include(includes);
            return query;
        }

        public async Task<IPagedEnumerable<Team>> SearchAsync(TeamSearchFilter filter, ResultListFilter resultFilter)
        {
            Guard.IsNotNull(filter, nameof(filter));
            Guard.IsNotNull(resultFilter, nameof(resultFilter));

            filter.Clean();

            using var db = await Factory.CreateDbContextAsync();

            IQueryable<Team> query = BuildQueryable(db, TeamQueryIncludeOption.All);

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(t => t.Name.Contains(filter.Name) || t.Mascot != null && t.Mascot.Contains(filter.Name));
            }

            var orderedQuery = resultFilter.Sorting.SortBy switch
            {
                "Name" => query.OrderBy(t => t.Name, resultFilter.Sorting.Direction)
                    .ThenBy(t => t.Mascot, resultFilter.Sorting.Direction),
                "City" => query.OrderBy(t => t.Stadium!.City!.Name, resultFilter.Sorting.Direction),
                "Division" => query.OrderBy(t => t.Division!.Name, resultFilter.Sorting.Direction),
                "Conference" => query.OrderBy(t => t.Division!.Conference!.Name, resultFilter.Sorting.Direction),
                _ => query.OrderBy(resultFilter.Sorting)
            };

            var results = await orderedQuery.Page(resultFilter.Paging, out int totalCount).ToListAsync();

            return new PagedList<Team>(results, totalCount);
        }
    }
}
