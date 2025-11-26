using Microsoft.Extensions.DependencyInjection;
using Common.Core.Domain;
using Common.Core.Validation;
using System;

namespace Common.Core
{
    public static class ServiceCollectionUserExtensions
    {
        /// <summary>
        /// Register current user to the collection under Scoped Lifetime.
        /// Type <typeparamref name="TUser"/> should be an interface or class type that represents the current user specific to the running application.
        /// Types <see cref="IUser"/> and <see cref="IUserId"/> will also be registerd as resolving the user via <typeparamref name="TUser"/> registration.
        /// </summary>
        /// <typeparam name="TUser">Interface or class that represents the user in the executing application. Should be specific to the application.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="implementationFactory">Factory function to resolve the current user. This is typically built off the HttpContext of the current request.</param>
        /// <returns></returns>
        public static IServiceCollection AddCurrentUser<TUser>(this IServiceCollection services, Func<IServiceProvider, TUser> implementationFactory)
            where TUser : class, IUser
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(implementationFactory, nameof(implementationFactory));

            services.AddScoped<TUser>(implementationFactory);
            services.AddScoped<IUserId>((serviceProvider) => serviceProvider.GetRequiredService<TUser>());
            services.AddScoped<IUser>((serviceProvider) => serviceProvider.GetRequiredService<TUser>());

            return services;
        }
    }
}