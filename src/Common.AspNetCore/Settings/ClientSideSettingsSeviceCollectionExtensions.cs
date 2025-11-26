using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.Configuration;

namespace Common.AspNetCore
{
    public static class ClientSideSettingsSeviceCollectionExtensions
    {
        /// <summary>
        /// Register <see cref="ClientSideSettings"/> with the service collection.
        /// Pulled from configuration under section <paramref name="sectionName"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">Established configuration from the executing application.</param>
        /// <param name="sectionName">Configuration section name for <see cref="ClientSideSettings"/></param>
        /// <returns></returns>
        public static IServiceCollection AddClientSideSettings(
            this IServiceCollection services, 
            IConfiguration configuration,
            string sectionName = "ClientSideSettings")
        {
            return services.AddCustomConfigurationSettings<ClientSideSettings>(configuration, sectionName);
        }

        /// <summary>
        /// Register static <see cref="ClientSideSettings"/> with the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settings">Settings value. If not provided, the defaults will be used.</param>
        /// <returns></returns>
        public static IServiceCollection AddClientSideSettings(
            this IServiceCollection services, 
            ClientSideSettings settings = null)
        {
            return services.AddSingleton<ClientSideSettings>(settings ?? new ClientSideSettings());
        }
    }
}
