using Common.Core.Domain;
using FootballSimulator.Application.Models;

namespace FootballSimulator.Application.Services
{
    public interface ITeamEditService
    {
        Task<TeamEditModel?> BuildModel(Guid? guid);
        Task<CommandResult> SaveAsync(TeamEditModel model);
        Task<CommandResult> DeleteAsync(Guid guid);
    }
}
