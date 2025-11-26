using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Common.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core
{
    public static class ServiceCollectionTypeRegistrationMatchExtensions
    {
        /// <summary>
        /// Add set of type matches to the service collection with specified lifetime.
        /// Useful for adding sets of matches after using reflection to query assemblies for common types.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="matches">List of type matches, declarations and associated implementations to register to the service collection.</param>
        /// <param name="lifetime">Lifetime for all type matches.</param>
        /// <param name="overwrite">Optionally overwrite any previously registered type match. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddTypeMatches(
            this IServiceCollection services,
            IEnumerable<TypeRegistrationMatch> matches, 
            ServiceLifetime lifetime,
            bool overwrite = false)
        {
            if (matches == null)
                return services;

            foreach (var match in matches)
            {
                if (!match.Implementation.GetCustomAttributes(typeof(DisableAutoServiceRegistrationAttribute), true).Any())
                {
                    if (overwrite)
                        services.Add(new ServiceDescriptor(match.Declaration, match.Implementation, lifetime));
                    else
                        services.TryAdd(new ServiceDescriptor(match.Declaration, match.Implementation, lifetime));
                }
            }

            return services;
        }
    }
}