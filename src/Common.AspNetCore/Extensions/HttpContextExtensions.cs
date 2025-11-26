using Microsoft.AspNetCore.Http;
using Common.Core.Validation;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore
{
    public static class HttpContextExtensions
    {
        public static string GetUniqueRequestId(this HttpContext context, string key = HttpContextConstants.UniqueRequestIdKey)
        {
            Guard.IsNotNull(context, nameof(context));
            Guard.IsNotNull(key, nameof(key));

            return context.Items[key]?.ToString();
        }

        /// <summary>
        /// Attempt to get a service from the request services. If found, returns true with the service object, otherwise false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContext"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool TryGetService<T>(this HttpContext httpContext, out T service)
        {
            service = httpContext.RequestServices.GetService<T>();
            return service != null;
        }

        /// <summary>
        /// Attempt to get services list from the request services. If one or more found, returns true with the services list, otherwise false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContext"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        public static bool TryGetServices<T>(this HttpContext httpContext, out IEnumerable<T> services)
        {
            services = httpContext.RequestServices.GetServices<T>();
            return services != null && services.Any();
        }
    }
}
