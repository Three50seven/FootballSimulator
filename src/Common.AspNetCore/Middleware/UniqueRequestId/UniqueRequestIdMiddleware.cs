using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Common.AspNetCore
{
    public class UniqueRequestIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UniqueRequestIdOptions _options;

        public UniqueRequestIdMiddleware(RequestDelegate next, IOptions<UniqueRequestIdOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);

            _next = next;
            _options = options.Value;

            if (string.IsNullOrWhiteSpace(_options.Key))
                _options.Key = UniqueRequestIdOptions.DefaultKey;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Items.TryGetValue(_options.Key, out object value) || _options.Override)
                context.Items[_options.Key] = Guid.NewGuid().ToString();

            await _next?.Invoke(context);
        }
    }
}
