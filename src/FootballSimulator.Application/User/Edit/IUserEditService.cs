using Common.Core.Domain;
using FootballSimulator.Application.Models;

namespace FootballSimulator.Application.Services
{
    public interface IUserEditService
    {
        Task<UserEditModel?> BuildModelAsync(Guid? guid);
        Task<CommandResult> SaveAsync(UserEditModel model);
        Task<CommandResult> DeleteAsync(Guid guid);
    }
}
