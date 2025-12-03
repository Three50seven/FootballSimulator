using Common.Core.Domain;
using FootballSimulator.Application.Models;

namespace FootballSimulator.Application.Services
{
    public class StadiumEditService : IStadiumEditService
    {
        public StadiumEditService() { }
        public Task<StadiumEditModel?> BuildModel(Guid? guid)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult> DeleteAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult> SaveAsync(StadiumEditModel model)
        {
            throw new NotImplementedException();
        }
    }
}
