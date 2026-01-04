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

            //// TODO: prevent extreme capacity values
            //if (entity.Capacity <= 0)
            //{
            //    brokenRules.Add("Stadium capacity must be a positive number.");
            //}

            //// TODO: Prevent duplicate stadium names within the same city
            //var existingStadium = _stadiumRepository.GetByNameAndCityId(entity.Name!, entity.CityId);
            //if (existingStadium != null && existingStadium.Id != entity.Id)
            //{
            //    brokenRules.Add("A stadium with the same name already exists in this city.");
            //}

            return brokenRules;
        }
    }
}
