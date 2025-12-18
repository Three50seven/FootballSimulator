using FootballSimulator.Application.Models;
using FootballSimulator.Core.DTOs;

namespace FootballSimulator.Application.Services
{
    public interface IStadiumManageModelService
    {
        Task<StadiumManageModel> BuildModelAsync();
        Task<StadiumSearchResultModel> SearchAsync(SearchQueryModel<StadiumSearchFilter> model);
    }
}
