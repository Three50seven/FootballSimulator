using Common.Core;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;
using System.Security.Claims;

namespace FootballSimulator.Application.User
{
    public class ApplicationUserPrincipal : ClaimsPrincipal, IApplicationUser
    {
        private Lazy<bool> _isAdmin;

        public ApplicationUserPrincipal(ClaimsPrincipal principal)
            : base(principal.SetEmptyIfNotAuthenticated())
        {
            _isAdmin = new Lazy<bool>(() => IsInRole(RoleOption.Admin));
        }

        protected string GetClaimValue(string typeName, bool checkMimic = false)
        {
            return FindFirst(typeName)?.Value ?? string.Empty;
        }

        public int UserId => GetClaimValue(IdentityConstants.UserId).ParseInteger();

        public Guid UserGuid => GetClaimValue(IdentityConstants.UserGuid).ParseGuid() ?? Guid.Empty;

        public string UserName => GetClaimValue(IdentityConstants.UserName);

        public UserName Name => new(GetClaimValue(IdentityConstants.FirstName),
                                             GetClaimValue(IdentityConstants.LastName));

        public int Id => UserId;

        public bool IsLoggedIn => Identity?.IsAuthenticated ?? false;

        public bool IsAdmin => _isAdmin.Value;

        public bool IsInRole(params RoleOption[] roles)
        {
            return roles.Any(r => base.IsInRole(r.AsFriendlyName()));
        }
    }
}
