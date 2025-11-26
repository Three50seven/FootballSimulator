using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Net.Http.Headers;

namespace Common.AspNetCore
{
    public class StaticFileCacheDefaultOptions : StaticFileOptions
    {
        public StaticFileCacheDefaultOptions(bool cacheResponse)
            : this (cacheResponse, StaticFilesExtensions.DefaultCacheControlAge)
        {

        }

        public StaticFileCacheDefaultOptions(bool cacheResponse, int cacheDuration)
            : this(cacheResponse, cacheDuration, new SharedOptions())
        {
            
        }

        public StaticFileCacheDefaultOptions(bool cacheResponse, int cacheDuration, SharedOptions sharedOptions)
            : base (sharedOptions)
        {
            if (cacheResponse)
            {
                cacheDuration = cacheDuration <= 0 ? StaticFilesExtensions.DefaultCacheControlAge : cacheDuration;

                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = $"public,max-age={cacheDuration}";
                };
            }
            else
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                    ctx.Context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                };
            }
        }
    }
}
