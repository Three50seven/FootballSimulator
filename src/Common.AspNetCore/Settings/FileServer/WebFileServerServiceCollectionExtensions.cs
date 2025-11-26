using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.AspNetCore.Services;
using Common.Configuration;
using Common.Core;
using Common.Core.Interfaces;
using Common.Core.Validation;

namespace Common.AspNetCore
{
    public static class WebFileServerServiceCollectionExtensions
    {
        /// <summary>
        /// Registers default web server file storage components with optional settings.
        /// Sets <see cref="IFileServerProvider"/> and <see cref="LocalWebServerPathResolver"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="fileServersConfigurationSettings">Custom settings for file servers.</param>
        /// <param name="fileStorageSettings">Optional file storage settings.</param>
        /// <returns></returns>
        public static IServiceCollection AddWebServerFileStorage(
            this IServiceCollection services,
            WebFileServersConfigurationSettings fileServersConfigurationSettings,
            FileStorageSettings fileStorageSettings = null)
        {
            services.AddSingleton<WebFileServersConfigurationSettings>(fileServersConfigurationSettings);

            AddWebServerFileStorage(services, fileStorageSettings);

            return services;
        }

        /// <summary>
        /// Registers default web server file storage components.
        /// Sets <see cref="IFileServerProvider"/> and <see cref="LocalWebServerPathResolver"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configuration">Established configuration from the executing application.</param>
        /// <param name="sectionName">Section name for required configuration section for <see cref="WebFileServersConfigurationSettings"/>.</param>
        /// <param name="fileStorageSettings">Optional file storage settings.</param>
        /// <returns></returns>
        public static IServiceCollection AddWebServerFileStorage(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = "FileServers",
            FileStorageSettings fileStorageSettings = null)
        {
            Guard.IsNotNull(services, nameof(services));

            services.AddCustomConfigurationSettings<WebFileServersConfigurationSettings>(configuration, sectionName);

            AddWebServerFileStorage(services, fileStorageSettings);

            return services;
        }

        private static IServiceCollection AddWebServerFileStorage(
            this IServiceCollection services,
            FileStorageSettings fileStorageSettings = null)
        {
            Guard.IsNotNull(services, nameof(services));

            services.AddLocalSystemFileStorage(absolute: false, storageSettings: fileStorageSettings);

            services.AddSingleton<IFileServerProvider, FileServerProvider>();
            services.AddSingleton<IFileSystemPathResolver, LocalWebServerPathResolver>();

            return services;
        }
    }
}
