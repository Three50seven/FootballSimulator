using Microsoft.Extensions.DependencyInjection;
using Common.Core;
using Common.Core.Validation;
using System.Text.Json;

namespace Common.Serialization.SystemTextJson
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="ISerializer"/> as System.Text.Json serializer <see cref="Serializer"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="options">Serialization options. Defaults to a new, default instance.</param>
        /// /// <returns></returns>
        public static IServiceCollection AddSystemTextSerializer(
            this IServiceCollection services,
            JsonSerializerOptions options = null!)
        {
            Guard.IsNotNull(services, nameof(services));

            options ??= Serializer.DefaultSettings;

            services.AddSingleton<JsonSerializerOptions>(options);
            services.AddSingleton<ISerializer, Serializer>();

            return services;
        }
    }
}
