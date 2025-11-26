using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IClaimsBuilder<T> where T : class
    {
        IEnumerable<Claim> Build(T user);
        Task<IEnumerable<Claim>> BuildAsync(T user);
    }
}
