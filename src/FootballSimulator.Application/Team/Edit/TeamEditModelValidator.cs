using Common.Core;
using Common.Core.Validation;
using FootballSimulator.Application.Models;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{
    public class TeamEditModelValidator : IValidator<TeamEditModel>
    {
        private readonly ITeamRepository _teamRepository;

        public TeamEditModelValidator(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public BrokenRulesList BrokenRules(TeamEditModel entity)
        {
            var brokenRules = new BrokenRulesList();

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                brokenRules.Add("Team name is required.");
            }
            else if (entity.Name.Length > 200)
            {
                brokenRules.Add("Stadium name cannot exceed 200 characters.");
            }
            if (entity.FoundedYear < 1850 || entity.FoundedYear > DateTime.Now.Year)
            {
                brokenRules.Add("Founded year must be between 1850 and the current year.");
            }
            //TODO: Add more validation rules as needed, e.g., checking if StadiumId and DivisionId exist in the database and prevent teams with the same name and same city etc. as other teams

            return brokenRules;
        }
    }
}
