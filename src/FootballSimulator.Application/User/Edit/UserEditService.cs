using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using FootballSimulator.Application.Models;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Application.Services
{
    public class UserEditService : IUserEditService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserEditModel> _validator;

        public UserEditService(IUserRepository userRepository, IValidator<UserEditModel> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<UserEditModel?> BuildModelAsync(Guid? guid)
        {
            if (!guid.HasValue)
                return new UserEditModel();

            var user = await _userRepository.GetByGuidAsync(guid.Value);

            if (user == null || user.IsNew)
                return null;

            return new UserEditModel
            {
                Id = user.Id,
                Guid = user.Guid,
                ApplicationUserId = user.ApplicationUserId,
                FirstName = user.Name.FirstName,
                LastName = user.Name.LastName,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<CommandResult> DeleteAsync(Guid guid)
        {
            try
            {
                var entity = await _userRepository.GetByGuidAsync(guid);

                if (entity == null)
                    throw new DataObjectNotFoundException(nameof(User), guid);

                await _userRepository.DeleteAsync(entity);

                return CommandResult.Success();
            }
            catch (ValidationException vex)
            {
                return CommandResult.Fail(vex.BrokenRules);
            }
        }

        public async Task<CommandResult> SaveAsync(UserEditModel model)
        {
            Guard.IsNotNull(model, nameof(model));

            try
            {
                _validator.Validate(model);

                Core.Domain.User? user = null;

                //strings are checked in validator and broken rules are returned...just satisfying C# here by wrapping in null checks
                if (model != null && model.FirstName != null && model.LastName != null && model.UserName != null && model.Email != null && model.ApplicationUserId != null)
                {
                    if (model.Guid.HasValue)
                    {
                        user = await _userRepository.GetByGuidAsync(model.Guid.Value);

                        if (user == null)
                            return CommandResult.Fail("User not found.");
                    }
                    else
                    {
                        user = await _userRepository.GetByApplicationIdentityAsync(model.UserName, includeArchived: true); // If the user already existed, reactivate them instead of making a new record

                        //create a new one - TODO: Copy over the username and application id/guid from the identity system (application_user)
                        if (user == null)
                            user = new Core.Domain.User(model.UserName.ToLower(), model.Email, new Name(model.FirstName, model.LastName), model.ApplicationUserId);
                    }

                    //otherwise we're just editing the user's editable info:
                    user.Name = new Name(model.FirstName, model.LastName);
                    user.Email = model.Email;

                    //currently only assigning one roll at launch, Admin, but leaving open for list of roles to be set:
                    IEnumerable<UserRole> userRoles = new List<UserRole>
                    {
                        new UserRole(user, RoleOption.Admin.ToInt())
                    };

                    user.SetUserRoles(userRoles);

                    await _userRepository.AddOrUpdateAsync(user);
                }
                
                return CommandResult.Success();
            }
            catch (ValidationException vex)
            {
                return CommandResult.Fail(vex.BrokenRules);
            }
        }
    }
}
