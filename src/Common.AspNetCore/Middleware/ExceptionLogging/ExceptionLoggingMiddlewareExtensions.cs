using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Common.Core.Validation;

namespace Common.AspNetCore
{
    public static class ExceptionLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Include middleware for simple exception logging.
        /// <see cref="ExceptionLoggingMiddleware"/> is registered with the <paramref name="app"/>.
        /// Logging is performed under <see cref="Microsoft.Extensions.Logging.ILogger{ExceptionLoggingMiddleware}"/>.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options">Options for the logging process, specifically <see cref="ExceptionLoggingOptions.ShouldLogException"/> callback option.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionLogging(
            this IApplicationBuilder app, 
            ExceptionLoggingOptions options = null)
        {
            Guard.IsNotNull(app, nameof(app));
            
            options ??= new ExceptionLoggingOptions();

            return app.UseMiddleware<ExceptionLoggingMiddleware>(Options.Create(options));
        }
    }
}
