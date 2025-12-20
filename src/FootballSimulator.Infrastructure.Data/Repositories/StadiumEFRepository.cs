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
        public StadiumEFRepository(FootballSimulatorDbContext dbContext, IEntityHistoryStore historyStore) 
            : base(dbContext, historyStore)
        {
        }
        
        protected override IQueryable<Stadium> FullEntitySet => base.FullEntitySet.IncludeAll();
        
        protected override IQueryable<Stadium> GetEntitySet(StadiumQueryIncludeOption includes)
        {
            return EntitySet.Include(includes);
        }
        
        public async Task<IPagedEnumerable<Stadium>> SearchAsync(StadiumSearchFilter filter, ResultListFilter resultFilter)
        {
            Guard.IsNotNull(filter, nameof(filter));
            Guard.IsNotNull(resultFilter, nameof(resultFilter));

            filter.Clean();

            IQueryable<Stadium> query = GetEntitySet(StadiumQueryIncludeOption.All);

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
                "LastUpdated" => query.OrderBy(p => p.ChangeEvents.Updated.Date, resultFilter.Sorting.Direction),
                _ => query.OrderBy(resultFilter.Sorting)
            };           

            var stadiums = await orderedQuery.Page(resultFilter.Paging, out int totalCount).ToListAsync();

            return new PagedList<Stadium>(stadiums, totalCount);
        }        
    }
}
