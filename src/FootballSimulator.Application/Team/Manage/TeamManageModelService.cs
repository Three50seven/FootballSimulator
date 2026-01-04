using Common.Core.Validation;
using FootballSimulator.Application.Models;
using FootballSimulator.Core.DTOs;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{
    public class TeamManageModelService : ITeamManageModelService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamManageModelService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<TeamSearchResultModel> SearchAsync(SearchQueryModel<TeamSearchFilter> model)
        {
            Guard.IsNotNull(model, nameof(model));
            model.Filter ??= new TeamSearchFilter();
            model.ResultFilter ??= new ResultListFilterModel();

            var resultFilter = model.ResultFilter.FilterValue;

            var results = await _teamRepository.SearchAsync(model.Filter, resultFilter);
            return new TeamSearchResultModel
            {
                Results = [.. results.Select(t => new TeamSearchListItem
                {
                    Id = t.Id,
                    Guid = t.Guid,
                    Name = t.Name,
                    FoundedYear = t.FoundedYear,
                    CityName = t.Stadium!.City!.Name,
                    ConferenceName = t.Division!.Conference!.Abbreviation,
                    DivisionName = t.Division.Name,
                    DisplayName = t.ToString(),
                    StadiumName = t.Stadium.Name
                })],
                Sorting = resultFilter.Sorting,
                Paging = new PagingNavigationModel(resultFilter.Paging, results.TotalCount)
            };
        }
    }
}
