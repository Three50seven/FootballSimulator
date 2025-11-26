using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;

namespace Common.AspNetCore
{
    public static class AjaxRedirectHeaderMiddlewareExtensions
    {
        /// <summary>
        /// For ajax requests, when response is set to redirect (302) and Location header exists, set a custom header with that redirect location 
        /// and change the response status code to 200.
        /// The custom redirect header should then be handled in the caller (js, jquery, etc.) to redirect the user accordingly to login page, etc.
        /// List of custom location urls can be set to only run this logic when attempting to redirect to those endpoints.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="resolveOptions">
        /// Whether the <see cref="AjaxRedirectHeaderOptions"/> should be pulled from the service collection. 
        /// This is the default setup so the options can and should be pulled from configuration.
        /// </param>
        /// <returns></returns>
        public static IApplicationBuilder UseAjaxRedirectHeader(this IApplicationBuilder app, bool resolveOptions = true)
        {
            ArgumentNullException.ThrowIfNull(app);

            // whether to have the required options object resolved from service collection or use a default value
            if (resolveOptions)
                return app.UseMiddleware<AjaxRedirectHeaderMiddleware>();
            else
                return UseAjaxRedirectHeader(app, new AjaxRedirectHeaderOptions());
        }

        /// <summary>
        /// For ajax requests, when response is set to redirect (302) and Location header exists, set a custom header with that redirect location 
        /// and change the response status code to 200.
        /// The custom redirect header should then be handled in the caller (js, jquery, etc.) to redirect the user accordingly to login page, etc.
        /// List of custom location urls can be set to only run this logic when attempting to redirect to those endpoints.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="locations">Custom list of endpoints for this logic to run against. When no locations supplied, all endpoints are processed.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAjaxRedirectHeader(this IApplicationBuilder app, IEnumerable<string> locations)
        {
            return UseAjaxRedirectHeader(app, new AjaxRedirectHeaderOptions()
            {
                Locations = locations ?? new List<string>()
            });
        }

        /// <summary>
        /// For ajax requests, when response is set to redirect (302) and Location header exists, set a custom header with that redirect location 
        /// and change the response status code to 200.
        /// The custom redirect header should then be handled in the caller (js, jquery, etc.) to redirect the user accordingly to login page, etc.
        /// List of custom location urls can be set to only run this logic when attempting to redirect to those endpoints.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options">Custom options. Allows for setting locations, headername, and the updated response code.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAjaxRedirectHeader(this IApplicationBuilder app, AjaxRedirectHeaderOptions options)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentNullException.ThrowIfNull(options);

            return app.UseMiddleware<AjaxRedirectHeaderMiddleware>(options);
        }
    }
}
