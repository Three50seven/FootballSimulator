using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using FootballSimulator.Application.Models;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{    
    public class TeamEditService : EditServiceBase<Team, ITeamRepository, TeamQueryIncludeOption>, ITeamEditService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<TeamEditModel> _validator;

        public TeamEditService(ITeamRepository teamRepository, IUnitOfWork unitOfWork, IValidator<TeamEditModel> validator)
            : base(teamRepository, unitOfWork)
        {
            _teamRepository = teamRepository;
            _validator = validator;
        }

        public async Task<TeamEditModel?> BuildModel(Guid? guid)
        {
            if (!guid.HasValue)
                return new TeamEditModel();

            var team = await _teamRepository.GetByGuidAsync(guid.Value);

            if (team == null || team.IsNew)
                return null;

            return new TeamEditModel
            {
                Id = team.Id,
                Guid = team.Guid,
                Name = team.Name,
                FoundedYear = team.FoundedYear,
                Mascot = team.Mascot,
                StadiumId = team.StadiumId,
                DivisionId = team.DivisionId
            };
        }

        public async Task<CommandResult> DeleteAsync(Guid guid)
        {
            try
            {
                var entity = await _teamRepository.GetByGuidAsync(guid);

                if (entity == null)
                    throw new DataObjectNotFoundException(nameof(Team), guid);

                await DeleteAsync(entity);

                return CommandResult.Success();
            }
            catch (ValidationException vex)
            {
                return CommandResult.Fail(vex.BrokenRules);
            }
        }

        public Task<CommandResult> SaveAsync(TeamEditModel model)
        {
            throw new NotImplementedException();
        }
    }
}
