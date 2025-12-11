using Common.Core;
using Common.Core.Validation;
using FootballSimulator.Application.Models;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{
    public class UserEditModelValidator : IValidator<UserEditModel>
    {
        private readonly IUserRepository _userRepository;

        public UserEditModelValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public BrokenRulesList BrokenRules(UserEditModel model)
        {
            Guard.IsNotNull(model, nameof(model));

            var brokenRules = new BrokenRulesList();

            if (string.IsNullOrWhiteSpace(model.FirstName))
                brokenRules.Add(new ValidationRule("First Name is required"));

            if (string.IsNullOrWhiteSpace(model.LastName))
                brokenRules.Add(new ValidationRule("Last Name is required"));

            if (string.IsNullOrWhiteSpace(model.Email))
                brokenRules.Add(new ValidationRule("Email is required"));

            if (string.IsNullOrWhiteSpace(model.UserName))
                brokenRules.Add(new ValidationRule("User Name is required"));

            if (model.FirstName?.Length > 100)
                brokenRules.Add(new ValidationRule("First Name cannot be more than 100 characters"));

            if (model.LastName?.Length > 100)
                brokenRules.Add(new ValidationRule("Last Name cannot be more than 100 characters"));

            if (model.UserName?.Length > 256)
                brokenRules.Add(new ValidationRule("User Name cannot be more than 256 characters"));

            if (model.Email?.Length > 256)
                brokenRules.Add(new ValidationRule("Email cannot be more than 256 characters"));

            if (model.UserName != null && !RegularExpressions.EmailRegex.IsMatch(model.UserName))
                brokenRules.Add(new ValidationRule("User Name must be in email format."));

            if (_userRepository.CheckForExistingUserName(model.UserName?.ToLower(), model.Id))
            {
                brokenRules.Add(new ValidationRule("A User with this User Name aleady exists."));
            }

            if (_userRepository.CheckForExistingEmail(model.Email?.ToLower(), model.Id))
            {
                brokenRules.Add(new ValidationRule("A User with this Email aleady exists."));
            }

            return brokenRules;
        }
    }
}
