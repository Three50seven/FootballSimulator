using Common.Configuration;
using Common.Core;
using Common.Core.Services;
using Common.Core.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Cache
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds data caching services to the existing service collection.
        /// Provides <see cref="ICache"/> implementation of <see cref="MemoryDataCache"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configuration">Established configuration from the executing application.</param>
        /// <param name="sectionName">Section name of <see cref="DataCacheSettings"/> found in appsetings.json configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddDataCache(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = "DataCache")
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(sectionName, nameof(sectionName));

            services.AddCustomConfigurationSettings<DataCacheSettings>(configuration, sectionName:
                                                                       sectionName,
                                                                       required: true,
                                                                       out DataCacheSettings settings);
            AddDataCacheService(services, settings.Enabled);

            return services;
        }

        /// <summary>
        /// Adds data caching services to the existing service collection.
        /// Provides <see cref="ICache"/> implementation of <see cref="MemoryDataCache"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="settings">Custom data cache settings.</param>
        /// <returns></returns>
        public static IServiceCollection AddDataCache(this IServiceCollection services, DataCacheSettings settings = null!)
        {
            Guard.IsNotNull(services, nameof(services));

            settings ??= new DataCacheSettings();

            services.AddSingleton<DataCacheSettings>(settings);
            AddDataCacheService(services, settings.Enabled);

            return services;
        }

        private static IServiceCollection AddDataCacheService(this IServiceCollection services, bool enabled)
        {
            if (enabled)
                return services.AddSingleton<ICache, MemoryDataCache>();
            else
                return services.AddSingleton<ICache, EmptyCache>();
        }
    }
}
