using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Common.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.AspNetCore
{
    public class AuthStatusHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthStatusHeaderOptions _options;

        public AuthStatusHeaderMiddleware(
            RequestDelegate next, 
            AuthStatusHeaderOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            _next = next;
            _options = options;
        }

        public Task Invoke(HttpContext context)
        {
            context?.Response?.OnStarting(state =>
            {
                var ctx = (HttpContext)state;
                if (ctx != null && !ctx.Response.HasStarted)
                {
                    bool authenticated = ctx.User?.Identity?.IsAuthenticated ?? false;
                    ctx.Response.Headers.TryAdd(_options.HeaderName, new StringValues(authenticated.ToLowerString()));
                }

                return Task.FromResult(0);

            }, context);

            return _next?.Invoke(context);
        }
    }
}
