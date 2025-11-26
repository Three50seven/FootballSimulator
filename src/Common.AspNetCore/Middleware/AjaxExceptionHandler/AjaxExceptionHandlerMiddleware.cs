using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Common.Core;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.AspNetCore
{
    /// <summary>
    /// Middleware for handling exceptions for ajax requests.
    /// Exceptions are logged under <see cref="ILogger{AjaxExceptionHandlerMiddleware}"/>.
    /// On exceptions for ajax requests, response is replaced with 500 response code and a friendly JSON body request.
    /// </summary>
    public class AjaxExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AjaxExceptionHandlerMiddleware> _logger;
        private readonly AjaxExceptionHandlerOptions _options;

        public AjaxExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<AjaxExceptionHandlerMiddleware> logger,
            IOptions<AjaxExceptionHandlerOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options?.Value ?? new AjaxExceptionHandlerOptions();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next?.Invoke(context);
            }
            catch (Exception ex)
            {
                if (!context.Request.IsAjax())
                {
                    _logger?.LogInformation("Caught exception ignored. Request not an ajax request.");
                    throw;
                }
                    
                _logger?.LogError(ex, "An unhandled exception has occurred on Ajax request: " + ex.Message);

                // Cannot do anything if the response has already started, just abort.
                if (context.Response.HasStarted)
                {
                    _logger?.LogWarning("The response has already started, the ajax error handler will not be executed.");
                    throw;
                }

                try
                {
                    // clear and set response to proper json error response
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    context.Response.OnStarting(state =>
                    {
                        ((HttpResponse)state)?.ClearCache();
                        return Task.CompletedTask;
                    }, context.Response);

                    var responseBody = BuildResponseBody(context, ex);
                    await context.Response.WriteAsync(responseBody, Encoding.UTF8);

                    return;
                }
                catch (Exception handlerEx)
                {
                    // Suppress secondary exceptions, re-throw the original.
                    _logger?.LogError(handlerEx, "An exception was thrown attempting to execute the ajax error handler.");
                }

                // Re-throw the original since it could not be handled
                throw; 
            }
        }

        private string BuildResponseBody(HttpContext context, Exception ex)
        {
            var message = ex.GetAjaxResponseMessage(debug: _options.IncludeExceptionMessage);
            var model = new ErrorInfo(message, GetUniqueId(context));

            var serializer = (ISerializer)context.RequestServices.GetService(typeof(ISerializer));
            if (serializer != null)
                return serializer.Serialize(model.ToFriendly());
            else
            {
                _logger?.LogWarning($"Serializer of type {typeof(ISerializer).FullName} was not found on request services list.");
                return ExceptionExtensions.UnexpectedError;
            }
        }

        private static Guid? GetUniqueId(HttpContext context)
        {
            var uniqueRequestId = context.GetUniqueRequestId();
            if (string.IsNullOrWhiteSpace(uniqueRequestId))
                return null;
            var id = uniqueRequestId.ParseGuid(allowEmpty: true, throwError: false);
            return (id == null || id == Guid.Empty) ? null : id;
        }
    }
}
