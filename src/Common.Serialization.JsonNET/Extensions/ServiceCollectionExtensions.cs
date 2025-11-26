using Microsoft.Extensions.DependencyInjection;
using Common.Core;
using Common.Core.Validation;
using Newtonsoft.Json;

namespace Common.Serialization.JsonNET
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add default Json.NET serializer as implementation for <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settings">Optional settings. Uses <see cref="JsonSerializerSettings"/> from default.</param>
        /// <returns></returns>
        public static IServiceCollection AddSerializer(
            this IServiceCollection services, 
            JsonSerializerSettings settings = null!)
        {
            Guard.IsNotNull(services, nameof(services));

            settings ??= Serializer.DefaultSettings;

            services.AddSingleton<JsonSerializerSettings>(settings);
            services.AddSingleton<ISerializer, Serializer>();

            return services;
        }
    }
}