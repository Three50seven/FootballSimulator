using Microsoft.AspNetCore.Routing;

namespace Common.AspNetCore
{
    public static class RouteDataExtensions
    {
        public static string GetControllerName(this RouteData routeData)
        {
            if (!routeData.Values.TryGetValue("controller", out object name))
                return null!;

            return name.ToString().TrimEnd("Controller".ToCharArray());
        }
    }
}
