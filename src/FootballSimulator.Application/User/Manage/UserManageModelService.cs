
using FootballSimulator.Application.Models;
using FootballSimulator.Core.DTOs;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{
    public class UserManageModelService : IUserManageModelService
    {
        private readonly IUserRepository _userRepository;

        public UserManageModelService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserManageModel> BuildModelAsync()
        {
            await Task.CompletedTask;
            return new UserManageModel();
        }

        public async Task<UserSearchResultModel> SearchAsync(SearchQueryModel<UserSearchFilter> model)
        {
            model.Filter ??= new UserSearchFilter();
            model.ResultFilter ??= new ResultListFilterModel();
            var resultFilter = model.ResultFilter.FilterValue;
            var results = await _userRepository.SearchAsync(model.Filter, resultFilter);
            return new UserSearchResultModel
            {
                Results = [.. results.Select(u => new UserSearchListItem
                {
                    Id = u.Id,
                    Guid = u.Guid,
                    FirstName = u.Name.FirstName,
                    LastName = u.Name.LastName,
                    UserName = u.UserName,
                    Email = u.Email
                })],
                Sorting = resultFilter.Sorting,
                Paging = new PagingNavigationModel(resultFilter.Paging, results.TotalCount)
            };
        }
    }
}
