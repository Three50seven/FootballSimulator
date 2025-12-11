using FootballSimulator.Application.Models;
using FootballSimulator.Core.DTOs;

namespace FootballSimulator.Application.Services
{
    public class UserManageModel : SearchModelBase<UserSearchListItem>
    {
        public UserSearchFilter Filter { get; set; } = new UserSearchFilter();
    }
}