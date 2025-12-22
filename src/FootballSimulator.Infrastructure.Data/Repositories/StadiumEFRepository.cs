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
    public class StadiumEFRepository : FootballSimulatorEntityRepositoryBase<Stadium, StadiumQueryIncludeOption>, IStadiumRepository
    {
        public StadiumEFRepository(IDbContextFactory<FootballSimulatorDbContext> factory, IEntityHistoryStore historyStore) 
            : base(factory, historyStore)
        {
        }

        protected override IQueryable<Stadium> BuildQueryable(FootballSimulatorDbContext db, StadiumQueryIncludeOption includes)
        {
            var query = base.BuildQueryable(db, includes);

            if (includes == StadiumQueryIncludeOption.All)
                query = query.IncludeAll();

            return query;
        }
        
        public async Task<IPagedEnumerable<Stadium>> SearchAsync(StadiumSearchFilter filter, ResultListFilter resultFilter)
        {
            Guard.IsNotNull(filter, nameof(filter));
            Guard.IsNotNull(resultFilter, nameof(resultFilter));

            filter.Clean();

            using var db = await Factory.CreateDbContextAsync();

            IQueryable<Stadium> query = base.BuildQueryable(db, StadiumQueryIncludeOption.All);

            if (filter.Name != null)
            {
                query = query.Where(s => s.Name != null && s.Name.ToLower().Contains(filter.Name));
            }

            if (filter.TypeId != null)
            {
                query = query.Where(s => s.StadiumTypeId == filter.TypeId);
            }

            if (filter.MinCapacity != null)
            {
                query = query.Where(s => s.Capacity >= filter.MinCapacity);
            }

            if (filter.MaxCapacity != null)
            {
                query = query.Where(s => s.Capacity <= filter.MaxCapacity);
            }

            if (filter.CityName != null)
            {
                query = query.Where(s => s.City! != null! && s.City.Name.ToLower().Contains(filter.CityName));
            }

            if (filter.TypeName != null)
            {
                query = query.Where(s => s.StadiumType! != null! && s.StadiumType.Name.ToLower().Contains(filter.TypeName));
            }

            var orderedQuery = resultFilter.Sorting.SortBy switch
            {
                "Type" => query.OrderBy(p => p.StadiumType, resultFilter.Sorting.Direction),
                "LastUpdated" => query.OrderBy(p => p.ChangeEvents.Updated.Date, resultFilter.Sorting.Direction),
                _ => query.OrderBy(resultFilter.Sorting)
            };           

            var stadiums = await orderedQuery.Page(resultFilter.Paging, out int totalCount).ToListAsync();            

            return new PagedList<Stadium>(stadiums, totalCount);
        }        
    }
}
