using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Common.AspNetCore
{
    public class AjaxRedirectHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AjaxRedirectHeaderMiddleware> _logger;
        private readonly AjaxRedirectHeaderOptions _options;

        public AjaxRedirectHeaderMiddleware(
            RequestDelegate next,
            ILogger<AjaxRedirectHeaderMiddleware> logger,
            AjaxRedirectHeaderOptions options = null)
        {
            _next = next;
            _logger = logger;
            _options = options ?? new AjaxRedirectHeaderOptions();
        }

        public Task Invoke(HttpContext context)
        {
            context?.Response?.OnStarting(state =>
            {
                var ctx = (HttpContext)state;
                if (ctx != null)
                {
                    if (ctx.Response.StatusCode == (int)HttpStatusCode.Redirect && ctx.Request.IsAjax())
                    {
                        if (ctx.Response.HasStarted)
                        {
                            _logger?.LogWarning("The response has already started, the AjaxRedirectHeaderMiddleware will not be executed.");
                            return Task.FromResult(0);
                        }

                        string location = ctx.Response.Headers.Location.ToString();

                        if (!string.IsNullOrWhiteSpace(location) && (!_options.Locations.Any() || _options.Locations.Contains(location)))
                        {
                            _logger?.LogInformation($@"Ajax request set to redirect to '{HttpUtility.UrlEncode(location)}' 
and will have custom redirect header '{_options.HeaderName}' applied and have response returned under {_options.StatusCodeForResponse} status code.");

                            if (!ctx.Response.Headers.ContainsKey(_options.HeaderName))
                                ctx.Response.Headers[_options.HeaderName] = new StringValues(location);

                            ctx.Response.StatusCode = _options.StatusCodeForResponse;
                        }
                    }
                }

                return Task.FromResult(0);

            }, context);

            return _next?.Invoke(context);
        }
    }
}
