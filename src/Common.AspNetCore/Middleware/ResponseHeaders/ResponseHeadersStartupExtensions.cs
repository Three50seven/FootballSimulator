using Microsoft.AspNetCore.Builder;
using Common.Core.Validation;

namespace Common.AspNetCore
{
    public static class ResponseHeadersStartupExtensions
    {
        /// <summary>
        /// Include middleware to adjust response headers. Designed to remove unwanted, typically auto-generated, headers.
        /// </summary>
        /// <param name="app">Existing application buider.</param>
        /// <param name="headersToRemove">List of response header names to remove from the HTTP response.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseResponseHeaderAdjustments(this IApplicationBuilder app, params string[] headersToRemove)
        {
            return UseResponseHeaderAdjustments(app, new ResponseHeadersMiddlewareOptions(headersToRemove, null));
        }

        /// <summary>
        /// Include middleware to adjust response headers. Designed to remove unwanted, typically auto-generated, headers.
        /// Can also add new headers to the HTTP response.
        /// </summary>
        /// <param name="app">Existing application buider.</param>
        /// <param name="options">Middleware options to designate new headers to add or headers to remove from the HTTP response.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseResponseHeaderAdjustments(this IApplicationBuilder app, ResponseHeadersMiddlewareOptions options)
        {
            Guard.IsNotNull(app, nameof(app));
            Guard.IsNotNull(options, nameof(options));

            return app.UseMiddleware<ResponseHeadersMiddleware>(options);
        }
    }
}
