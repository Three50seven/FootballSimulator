using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Common.Core.Validation;

namespace Common.AspNetCore.Mvc
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add MVC's <see cref="IUrlHelper"/> to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUrlHelper(this IServiceCollection services)
        {
            Guard.IsNotNull(services, nameof(services));

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddScoped<IUrlHelper>(serviceProvider =>
            {
                var factory = serviceProvider.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(serviceProvider.GetRequiredService<IActionContextAccessor>().ActionContext);
            });

            return services;
        }
    }
}
