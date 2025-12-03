using FootballSimulator.Core.Domain;
using System.Security.Claims;

namespace FootballSimulator.Core.Interfaces
{
    public interface IUserClaimsService
    {
        IEnumerable<Claim> BuildClaims(User user, string applicationUserGuid);
    }
}
