using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace Common.AspNetCore
{
    public static class UniqueRequestIdExtensions
    {
        public static IApplicationBuilder UseUniqueRequestId(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            return app.UseMiddleware<UniqueRequestIdMiddleware>();
        }

        public static IApplicationBuilder UseUniqueRequestId(this IApplicationBuilder app, string key)
        {
            ArgumentNullException.ThrowIfNull(app);

            return app.UseUniqueRequestId(new UniqueRequestIdOptions()
            {
                Key = key
            });
        }

        public static IApplicationBuilder UseUniqueRequestId(this IApplicationBuilder app, UniqueRequestIdOptions options)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentNullException.ThrowIfNull(options);

            return app.UseMiddleware<UniqueRequestIdMiddleware>(Options.Create(options));
        }
    }
}
