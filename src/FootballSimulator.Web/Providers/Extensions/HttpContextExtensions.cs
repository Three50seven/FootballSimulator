using Common.Core.Validation;
using FootballSimulator.Application.User;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Web
{
    public static class HttpContextExtensions
    {
        public static IApplicationUser ApplicationUser(this HttpContext httpContext)
        {
            Guard.IsNotNull(httpContext, nameof(httpContext));

            return new ApplicationUserPrincipal(httpContext.User);
        }
    }
}
