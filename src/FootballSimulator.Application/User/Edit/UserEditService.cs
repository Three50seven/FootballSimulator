using Common.Core.Domain;
using FootballSimulator.Application.Models;

namespace FootballSimulator.Application.Services
{
    public class UserEditService : IUserEditService
    {
        public Task<UserEditModel?> BuildModelAsync(Guid? guid)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult> DeleteAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult> SaveAsync(UserEditModel model)
        {
            throw new NotImplementedException();
        }
    }
}
