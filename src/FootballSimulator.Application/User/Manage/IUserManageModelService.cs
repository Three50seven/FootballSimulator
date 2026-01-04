using FootballSimulator.Application.Models;
using FootballSimulator.Core.DTOs;

namespace FootballSimulator.Application.Services
{
    public interface IUserManageModelService
    {
        Task<UserManageModel> BuildModelAsync();
        Task<UserSearchResultModel> SearchAsync(SearchQueryModel<UserSearchFilter> model);
    }
}
