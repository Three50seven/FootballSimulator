using Microsoft.AspNetCore.Builder;
using System;

namespace Common.AspNetCore
{
    public static class AuthStatusHeaderMiddlewareExtensions
    {
        /// <summary>
        /// Sets response header that represents the user's authentication status.
        /// Designed to be used in ajax responses to provide a check on user's authentication status on the response.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthStatusHeader(this IApplicationBuilder app)
        {
            return UseAuthStatusHeader(app, new AuthStatusHeaderOptions());
        }

        /// <summary>
        /// Sets response header that represents the user's authentication status.
        /// Designed to be used in ajax responses to provide a check on user's authentication status on the response.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="headerName">Name of the header to use. Value of <see cref="AuthStatusHeaderOptions.DefaultHeaderName"/> is used for the header name by default.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthStatusHeader(this IApplicationBuilder app, string headerName)
        {
            return UseAuthStatusHeader(app, new AuthStatusHeaderOptions()
            {
                HeaderName = headerName
            });
        }

        /// <summary>
        /// Sets response header that represents the user's authentication status.
        /// Designed to be used in ajax responses to provide a check on user's authentication status on the response. 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options">Options including the header name to use.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthStatusHeader(this IApplicationBuilder app, AuthStatusHeaderOptions options)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentNullException.ThrowIfNull(options);

            return app.UseMiddleware<AuthStatusHeaderMiddleware>(options);
        }
    }
}
