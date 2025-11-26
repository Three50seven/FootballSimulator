using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.Configuration;
using Common.Core.Validation;
using System;
using System.IO;

namespace Common.AspNetCore
{
    public static class DataProtectionServiceCollectionExtensions
    {
        /// <summary>
        /// Include Microsoft DataProtection protocols with specific application name and persisiting keys to the file system.
        /// Requires applicaiton configuration with a section for <see cref="DataProtectionSettings"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">Application's configuration.</param>
        /// <param name="sectionName">Section name of the DataProtection settings. Defaults to "DataProtection".</param>
        /// <param name="dataProtectionBuilder">Optional callback to customize the built-in dataprotection configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddDataProtection(
            this IServiceCollection services, 
            IConfiguration configuration,
            string sectionName = "DataProtection",
            Action<IDataProtectionBuilder> dataProtectionBuilder = null)
        {
            Guard.IsNotNull(configuration, nameof(configuration));
            Guard.IsNotNull(sectionName, nameof(sectionName));

            services.AddCustomConfigurationSettings<DataProtectionSettings>(configuration, sectionName,
                required: true, out DataProtectionSettings dataProtectionSettings);

            return AddDataProtection(services, dataProtectionSettings, dataProtectionBuilder);
        }

        /// <summary>
        /// Include Microsoft DataProtection protocols with specific application name and persisiting keys to the file system.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settings">Custom settings for the application name and key storage directory.</param>
        /// <param name="dataProtectionBuilder">Optional callback to customize the built-in dataprotection configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddDataProtection(
            this IServiceCollection services,
            DataProtectionSettings settings,
            Action<IDataProtectionBuilder> dataProtectionBuilder = null)
        {
            Guard.IsNotNull(settings, nameof(settings));

            var builder = services.AddDataProtection()
                                  .SetApplicationName(settings.ApplicationName)
                                  .PersistKeysToFileSystem(new DirectoryInfo(settings.KeyStorageDirectory));

            dataProtectionBuilder?.Invoke(builder);

            return services;
        }
    }
}
