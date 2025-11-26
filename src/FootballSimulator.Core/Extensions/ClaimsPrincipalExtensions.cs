using System.Security.Claims;

namespace FootballSimulator
{
    public static class ClaimsPrincipalExtensions
    {
        public static ClaimsPrincipal SetEmptyIfNotAuthenticated(this ClaimsPrincipal principal)
        {
            if (principal.Identity is not null && !principal.Identity.IsAuthenticated)
            {
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
            return principal;
        }

        public static ClaimsPrincipal ToEmpty(this ClaimsPrincipal principal)
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        public static ClaimsIdentity? GetClaimsIdentity(this ClaimsPrincipal principal, string scheme)
        {
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)            
                return null;

            return principal.Identities.FirstOrDefault(i => i.AuthenticationType == scheme);
        }
    }
}
