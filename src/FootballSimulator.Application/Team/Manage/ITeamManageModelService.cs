using FootballSimulator.Application.Models;
using FootballSimulator.Core.DTOs;

namespace FootballSimulator.Application.Services
{
    public interface ITeamManageModelService
    {
        Task<TeamSearchResultModel> SearchAsync(SearchQueryModel<TeamSearchFilter> model);
    }
}
