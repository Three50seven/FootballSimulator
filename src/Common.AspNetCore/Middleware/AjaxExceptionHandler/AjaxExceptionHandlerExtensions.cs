using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Common.Core.Validation;

namespace Common.AspNetCore
{
    public static class AjaxExceptionHandlerExtensions
    {
        /// <summary>
        /// Register middleware for handling exceptions on Ajax requests.
        /// Middleware <see cref="AjaxExceptionHandlerMiddleware"/> is registered to the pipeline.
        /// Exception message will be included in response if environment is Development.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options">Optional custom options, specifically to indicate if exception message should be included in response.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAjaxExceptionHandler(this IApplicationBuilder app, AjaxExceptionHandlerOptions options = null)
        {
            Guard.IsNotNull(app, nameof(app));

            if (options == null)
            {
                var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                options = new AjaxExceptionHandlerOptions()
                {
                    IncludeExceptionMessage = environment.IsDevelopment()
                };
            }

            return app.UseMiddleware<AjaxExceptionHandlerMiddleware>(Options.Create(options));
        }
    }
}
