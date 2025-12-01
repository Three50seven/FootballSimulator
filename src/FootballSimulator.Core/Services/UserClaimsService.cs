using Common.Core;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;
using System.Security.Claims;

namespace FootballSimulator.Core.Services
{
    public class UserClaimsService : IUserClaimsService
    {
        public IEnumerable<Claim> BuildClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(IdentityConstants.UserId, user.Id.ToString()),
                new Claim(IdentityConstants.UserName, user.UserName ?? string.Empty),
                new Claim(IdentityConstants.Email, user.Email ?? ""),
                new Claim(IdentityConstants.FirstName, user.Name.FirstName ?? ""),
                new Claim(IdentityConstants.LastName, user.Name.LastName ?? ""),
                new Claim(IdentityConstants.UserGuid, user.Guid.ToString())
            };

            foreach (var userRole in user.UserRoles)
            {
                if (Enum.IsDefined(typeof(RoleOption), userRole.RoleId))
                {
                    var role = (RoleOption)userRole.RoleId;
                    claims.Add(new Claim(ClaimTypes.Role, role.AsFriendlyName()));
                }
            }

            return claims;
        }
    }
}
