using Common.Core.Domain;
using Common.Core.Validation;
using FootballSimulator.Application.Services;
using FootballSimulator.Core.Interfaces;
using FootballSimulator.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FootballSimulator.Web.Providers
{
    public class IdentitySignOnService : IIdentitySignOnService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IUserClaimsService _userClaimsService;
        private readonly IUserLoginRecorder _userLoginRecorder;

        public IdentitySignOnService(
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IUserClaimsService userClaimsService,
            IUserLoginRecorder userLoginRecorder,
            FootballSimulatorDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _userClaimsService = userClaimsService;
            _userLoginRecorder = userLoginRecorder;
        }

        public async Task<CommandResult> LoginAsync(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName)) {
                    return CommandResult.Fail("Invalid User Name.");
                }
                var user = await _userRepository.GetByApplicationIdentityAsync(userName);
                if (user! == null!)
                {
                    return CommandResult.Fail("User not found.");
                }

                var claims = _userClaimsService.BuildClaims(user);
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext != null)
                {
                    var authProperties = new AuthenticationProperties();
                    authProperties.AllowRefresh = false;
                    authProperties.IssuedUtc = DateTime.UtcNow;
                    authProperties.IsPersistent = true;

                    await httpContext.SignInAsync(
                    AuthConstants.DefaultScheme,
                        new ClaimsPrincipal(new ClaimsIdentity(claims, AuthConstants.DefaultScheme)),
                        authProperties);

                    await _userLoginRecorder.RecordAsync(user.Id, httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
                }
                return CommandResult.Success();

            }
            catch (ValidationException vex)
            {
                return CommandResult.Fail(vex.BrokenRules);
            }
        }

        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                await httpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                await httpContext.SignOutAsync(AuthConstants.DefaultScheme);
            }
        }
    }
}
