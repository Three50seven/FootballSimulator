using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Common.AspNetCore
{
    /// <summary>
    /// Initial implementation copied from <see cref="Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"/>.
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlerOptions _options;
        private readonly ILogger _logger;
        private readonly DiagnosticSource _diagnosticSource;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IOptions<ExceptionHandlerOptions> options,
            DiagnosticSource diagnosticSource)
        {
            _next = next;
            _options = options.Value;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
            _diagnosticSource = diagnosticSource;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "An unhandled exception has occurred: " + ex.Message);

                // We can't do anything if the response has already started, just abort.
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                PathString originalPath = context.Request.Path;
                if (_options.ExceptionHandlingPath.HasValue)
                {
                    context.Request.Path = _options.ExceptionHandlingPath;
                }

                try
                {
                    context.Response.Clear();
                    var exceptionHandlerFeature = new ExceptionHandlerFeature()
                    {
                        Error = ex,
                        Path = originalPath.Value,
                    };
                    context.Features.Set<IExceptionHandlerFeature>(exceptionHandlerFeature);
                    context.Features.Set<IExceptionHandlerPathFeature>(exceptionHandlerFeature);
                    context.Response.StatusCode = 500;

                    // set response to have no cache declarations
                    context.Response.OnStarting(state =>
                    {
                        ((HttpResponse)state)?.ClearCache();
                        return Task.CompletedTask;
                    }, context.Response);

                    if (_options.ExceptionHandler != null)
                        await _options.ExceptionHandler(context);

                    if (_diagnosticSource.IsEnabled("Microsoft.AspNetCore.Diagnostics.HandledException"))
                    {
                        _diagnosticSource.Write("Microsoft.AspNetCore.Diagnostics.HandledException", new { httpContext = context, exception = ex });
                    }

                    return;
                }
                catch (Exception ex2)
                {
                    // Suppress secondary exceptions, re-throw the original.
                    _logger.LogError(0, ex2, "An exception was thrown attempting to execute the error handler.");
                }
                finally
                {
                    context.Request.Path = originalPath;
                }

                // Re-throw the original if it could not be handled here
                throw; 
            }
        }
    }
}
