using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Common.Core.Validation;
using Microsoft.AspNetCore.Http;

namespace Common.AspNetCore
{
    /// <summary>
    /// Middleware for simple exception logging. Uncaught exceptions will be logged under <see cref="ILogger{ExceptionLoggingMiddleware}"/>.
    /// </summary>
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;
        private readonly ExceptionLoggingOptions _options;

        public ExceptionLoggingMiddleware(RequestDelegate next, 
            ILogger<ExceptionLoggingMiddleware> logger,
            IOptions<ExceptionLoggingOptions> options)
        {
            Guard.IsNotNull(logger, nameof(logger));

            _next = next;
            _logger = logger;
            _options = options?.Value ?? new ExceptionLoggingOptions();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                if (_options.ShouldLogException?.Invoke(context, ex) ?? true)
                    _logger.LogError(ex, "An unhandled exception has occurred: " + ex.Message);

                throw;
            }           
        }
    }
}
