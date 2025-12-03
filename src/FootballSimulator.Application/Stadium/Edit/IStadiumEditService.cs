using Common.Core.Domain;
using FootballSimulator.Application.Models;

namespace FootballSimulator.Application.Services
{
    public interface IStadiumEditService
    {
        Task<StadiumEditModel?> BuildModel(Guid? guid);
        Task<CommandResult> SaveAsync(StadiumEditModel model);
        Task<CommandResult> DeleteAsync(Guid guid);
    }
}
