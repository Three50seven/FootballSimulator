using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Common.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.AspNetCore.Mvc
{
    public static class ValidationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds custom adapter provider class <see cref="ValidationAttributeAdapterProvider"/> around the default instance and allows
        /// for a custom type lookup via <paramref name="adapterFactoryLookup"/> to lookup a factory to create custom <see cref="IAttributeAdapter"/>.
        /// This is used for customizing a <see cref="ValidationAttribute"/> for client-side setup.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="adapterFactoryLookup">
        /// Required list of lookups. The type will be a <see cref="ValidationAttribute"/> 
        /// and should return a factory to create the <see cref="IAttributeAdapter"/>. 
        /// This adapter will be used in place of the adapter that is found by the default provider.
        /// If the type is not found, the default provider will be used.
        /// </param>
        /// <returns></returns>
        public static IServiceCollection AddValidationAttributeAdapterProvider(
            this IServiceCollection services,
            IReadOnlyDictionary<Type, IValidationAttributeAdapterFactory> adapterFactoryLookup)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(adapterFactoryLookup, nameof(adapterFactoryLookup));

            services.AddSingleton<IReadOnlyDictionary<Type, IValidationAttributeAdapterFactory>>(adapterFactoryLookup);

            return services.AddSingleton<IValidationAttributeAdapterProvider>((serviceProvider) =>
            {
                return new ValidationAttributeAdapterProvider(
                    new Microsoft.AspNetCore.Mvc.DataAnnotations.ValidationAttributeAdapterProvider(),
                    serviceProvider.GetRequiredService<IReadOnlyDictionary<Type, IValidationAttributeAdapterFactory>>());
            });
        }
    }
}
