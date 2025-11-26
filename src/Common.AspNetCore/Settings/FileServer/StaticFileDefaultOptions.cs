using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace Common.AspNetCore
{
    /// <summary>
    /// Default static file serving options used for the default, static files in aspnetcore application, typically the wwwroot folder.
    /// Provides options for caching and authentication requirements.
    /// </summary>
    public class StaticFileDefaultOptions : StaticFileOptions
    {
        public StaticFileDefaultOptions(
            bool cacheResponse = false, 
            int cacheDuration = 0, 
            bool requireAuthentication = false,
            SharedOptions sharedOptions = null)
            : base(sharedOptions ?? new SharedOptions())
        {
            if (cacheResponse)
            {
                cacheDuration = cacheDuration <= 0 ? StaticFilesExtensions.DefaultCacheControlAge : cacheDuration;

                OnPrepareResponse += ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = $"public,max-age={cacheDuration}";
                };
            }
            else
            {
                OnPrepareResponse += ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                    ctx.Context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                };
            }

            if (requireAuthentication)
            {
                OnPrepareResponse += ctx =>
                {
                    if (ctx.Context.User.Identity.IsAuthenticated)
                        return;

                    ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                    ctx.Context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                };
            }
        }
    }
}
