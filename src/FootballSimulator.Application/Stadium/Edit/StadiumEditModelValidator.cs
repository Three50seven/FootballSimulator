using Common.Core;
using Common.Core.Validation;
using FootballSimulator.Application.Models;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{
    public class StadiumEditModelValidator : IValidator<StadiumEditModel>
    {
        private readonly IStadiumRepository _stadiumRepository;

        public StadiumEditModelValidator(IStadiumRepository stadiumRepository)
        {
            _stadiumRepository = stadiumRepository;
        }

        public BrokenRulesList BrokenRules(StadiumEditModel entity)
        {
            var brokenRules = new BrokenRulesList();

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                brokenRules.Add("Stadium name is required.");
            }
            else if (entity.Name.Length > 200)
            {
                brokenRules.Add("Stadium name cannot exceed 200 characters.");
            }

            return brokenRules;
        }
    }
}
