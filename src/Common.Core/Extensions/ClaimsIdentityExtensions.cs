using Common.Core.Validation;
using System.Security.Claims;

namespace Common.Core
{
    public static class ClaimsIdentityExtensions
    {
        /// <summary>
        /// Checks for existing claim and removes it if exists. Then addes new claim.
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
        /// Removes claim with the given key <paramref name="key"/>.
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
