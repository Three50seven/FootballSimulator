using Common.Core.Validation;
using FootballSimulator.Application.Models;
using FootballSimulator.Core.DTOs;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{
    public class StadiumManageModelService : IStadiumManageModelService
    {
        private readonly IStadiumRepository _stadiumRepository;

        public StadiumManageModelService(IStadiumRepository stadiumRepository)
        {
            _stadiumRepository = stadiumRepository;
        }

        public async Task<StadiumManageModel> BuildModelAsync()
        {
            await Task.CompletedTask;
            return new StadiumManageModel();
        }

        public async Task<StadiumSearchResultModel> SearchAsync(SearchQueryModel<StadiumSearchFilter> model)
        {
            Guard.IsNotNull(model, nameof(model));

            model.Filter ??= new StadiumSearchFilter();
            model.ResultFilter ??= new ResultListFilterModel();

            var resultFilter = model.ResultFilter.FilterValue;

            var results = await _stadiumRepository.SearchAsync(model.Filter, resultFilter);

            return new StadiumSearchResultModel
            {
                Results = [.. results.Select(s => new StadiumSearchListItem
                {
                    Id = s.Id,
                    Guid = s.Guid,
                    Name = s.Name,
                    Capacity = s.Capacity,
                    CityName = s.City?.Name,
                    StadiumTypeName = s.StadiumType?.Name,
                    ClimateTypeName = s.ClimateType?.Name
                })],
                Sorting = resultFilter.Sorting,
                Paging = new PagingNavigationModel(resultFilter.Paging, results.TotalCount)
            };
        }
    }
}
