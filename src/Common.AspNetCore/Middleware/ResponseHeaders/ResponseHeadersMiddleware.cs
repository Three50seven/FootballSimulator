using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.AspNetCore
{
    /// <summary>
    /// Middleware to adjust response headers. Designed to remove unwanted, typically auto-generated, headers. 
    /// Can also add new headers to the HTTP response.
    /// </summary>
    public class ResponseHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ResponseHeadersMiddlewareOptions _options;

        public ResponseHeadersMiddleware(RequestDelegate next, ResponseHeadersMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        public Task Invoke(HttpContext context)
        {
            if (_options.AdjustResponseHeaders)
            {
                context?.Response?.OnStarting(state =>
                {
                    var ctx = (HttpContext)state;
                    if (ctx != null)
                    {
                        for (int i = 0; i < _options.HeadersToRemove.Length; i++)
                        {
                            if (ctx.Response.Headers.ContainsKey(_options.HeadersToRemove[i]))
                            {
                                ctx.Response.Headers.Remove(_options.HeadersToRemove[i]);
                            }
                        }

                        for (int i = 0; i < _options.HeadersToAdd.Length; i++)
                        {
                            if (!ctx.Response.Headers.ContainsKey(_options.HeadersToAdd[i].Key))
                            {
                                ctx.Response.Headers[_options.HeadersToAdd[i].Key] = _options.HeadersToAdd[i].Value;
                            }
                        }
                    }

                    return Task.FromResult(0);

                }, context);
            }
                
            return _next?.Invoke(context);
        }
    }
}
