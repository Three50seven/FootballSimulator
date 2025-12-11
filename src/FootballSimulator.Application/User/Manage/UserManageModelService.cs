
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
            var model = new UserManageModel();

            var users = await _userRepository.GetAllAsync();
            model.Results = [.. users.Select(u => new UserSearchListItem
            {
                Id = u.Id,
                Guid = u.Guid,
                FirstName = u.Name.FirstName,
                LastName = u.Name.LastName,
                UserName = u.UserName,
                Email = u.Email
            })];

            await Task.CompletedTask;

            return model;
        }
    }
}
