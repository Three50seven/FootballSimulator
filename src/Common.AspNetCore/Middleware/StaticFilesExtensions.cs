using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Common.AspNetCore
{
    public static class StaticFilesExtensions
    {
        public static int DefaultCacheControlAge = 604800; // 7 days

        /// <summary>
        /// Returns static files if found for a given request using AspNetCore's <see cref="StaticFileExtensions.UseStaticFiles(IApplicationBuilder)"/>.
        /// Allows for setting whether or not static file responses should include cache headers and allow for specifying duration.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="duration">Cache duration. If 0, value from <see cref="DefaultCacheControlAge"/> will be used.</param>
        /// <param name="cacheResponse">Whether or not static file responses should be cached. If not specified, cache will be enabled in Production environment only.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticFilesWithCacheResponse(
            this IApplicationBuilder app, 
            int duration = default, 
            bool? cacheResponse = null)
        {
            if (duration == default)
                duration = DefaultCacheControlAge;

            if (cacheResponse == null)
            {
                var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                cacheResponse = environment.IsProduction();
            }

            return app.UseStaticFiles(new StaticFileCacheDefaultOptions(cacheResponse.Value, duration));
        }
    }
}
