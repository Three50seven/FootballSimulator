using Common.Core.Validation;
using System.Linq;
using System.Security.Claims;

namespace Common.Core
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Return identity from the principal under the provided scheme.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static ClaimsIdentity GetClaimsIdentity(this ClaimsPrincipal principal, string scheme)
        {
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                return null;

            return principal.Identities.FirstOrDefault(i => i.AuthenticationType == scheme);
        }

        /// <summary>
        /// Adds claim key/value pair to the list of claims on the identity.
        /// If a claim with the same key already exists on the identity, that claim is replaced with a new claim and value.
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ClaimsIdentity AddUpdateClaim(this ClaimsIdentity claimsIdentity, string key, string value)
        {
            Guard.IsNotNull(claimsIdentity, nameof(claimsIdentity));
            Guard.IsNotNull(key, nameof(key));

            var existingClaim = claimsIdentity.FindFirst(key);
            if (existingClaim != null)
                claimsIdentity.RemoveClaim(existingClaim);

            claimsIdentity.AddClaim(new Claim(key, value.SetNullToEmpty()));

            return claimsIdentity;
        }

        /// <summary>
        /// Removes first instance of a claim from the list of claims on the identity based on the provided key. 
        /// If claim is not found, the action is ignored.
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ClaimsIdentity RemoveClaim(this ClaimsIdentity claimsIdentity, string key)
        {
            Guard.IsNotNull(claimsIdentity, nameof(claimsIdentity));
            Guard.IsNotNull(key, nameof(key));

            var existingClaim = claimsIdentity.FindFirst(key);
            if (existingClaim != null)
                claimsIdentity.RemoveClaim(existingClaim);

            return claimsIdentity;
        }
    }
}
