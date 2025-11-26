using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Common.Core;
using Common.Core.Services;
using Common.Core.Validation;
using System;

namespace Common.Configuration
{
    public static class ConfigurationServiceCollectionExtensions
    {
        /// <summary>
        /// Register a custom class that represents a given configuration section on the <paramref name="configuration"/> instance.
        /// </summary>
        /// <typeparam name="TSettingsType">Class that represents the configuration section.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configuration">Established configuration on the running application.</param>
        /// <param name="sectionName">Name of the section found in <paramref name="configuration"/> that will be applied to <typeparamref name="TSettingsType"/>.</param>
        /// <param name="required">Whether or not the section indicated by <paramref name="sectionName"/> is a required section in the configuration. Defaults to true.</param>
        /// <param name="lifeTime">Registration lifetime. Defaults to singleton. If not singleton, configuration will reload when changed.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfigurationSettings<TSettingsType>(
            this IServiceCollection services, 
            IConfiguration configuration,
            string sectionName, 
            bool required = true,
            ServiceLifetime lifeTime = ServiceLifetime.Singleton)
            where TSettingsType : class, new()
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(configuration, nameof(configuration));

            var section = configuration.GetSection(sectionName, required: required);

            return AddCustomConfigurationSettings<TSettingsType>(services, 
                                                                 section, 
                                                                 required: required, 
                                                                 lifeTime: lifeTime);
        }

        /// <summary>
        /// Register a custom class that represents a given configuration section on the <paramref name="configuration"/> instance.
        /// </summary>
        /// <typeparam name="TSettingsType">Class that represents the configuration section.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configuration">Established configuration on the running application.</param>
        /// <param name="sectionName">Name of the section found in <paramref name="configuration"/> that will be applied to <typeparamref name="TSettingsType"/>.</param>
        /// <param name="required">Whether or not the section indicated by <paramref name="sectionName"/> is a required section in the configuration.</param>
        /// <param name="settings">Out bound parameter that holds the custom, bound settings class of type <typeparamref name="TSettingsType"/> loaded from configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfigurationSettings<TSettingsType>(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName,
            bool required,
            out TSettingsType settings)
            where TSettingsType : class, new()
        {
            AddCustomConfigurationSettings<TSettingsType>(services, 
                                                          configuration, 
                                                          sectionName, 
                                                          required: required, 
                                                          lifeTime: ServiceLifetime.Singleton);

            settings = new TSettingsType();

            // NOTE: this exists check may not be necessary 
            //       doing it here because the "required" option may be set to false
            //       and not sure if there should be a check before calling "bind"
            if (configuration.SectionExists(sectionName))
                configuration.Bind(sectionName, settings);
            
            return services;
        }

        /// <summary>
        /// Register a custom class based on the configuration section <paramref name="configurationSection"/>.
        /// </summary>
        /// <typeparam name="TSettingsType">Class that represents the configuration section.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configurationSection">Retuired specific section from the established configuration on the running application.</param>
        /// <param name="required">Whether or not the configuration section is a required section in the configuration. Defaults to true.</param>
        /// <param name="lifeTime">Registration lifetime. Defaults to singleton. If not singleton, configuration will reload when changed.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfigurationSettings<TSettingsType>(
            this IServiceCollection services,
            IConfigurationSection configurationSection,
            bool required = true,
            ServiceLifetime lifeTime = ServiceLifetime.Singleton)
            where TSettingsType : class, new()
        {
            Guard.IsNotNull(services, nameof(services));
            
            if (configurationSection == null || !configurationSection.Exists())
            {
                if (required)
                    throw new ConfigurationSectionException($"Required configuration section '{configurationSection?.Key}' was not found.");

                services.Add(new ServiceDescriptor(typeof(TSettingsType), typeof(TSettingsType), lifeTime));
                return services;
            }

            services.Configure<TSettingsType>(configurationSection);

            // Register as singleton or scoped. 
            // Singleton is one instance for the app. 
            // Scoped settings will be read on every request (no app restart on setting changes at the drawback of performance)
            // IOptionsSnapshot is also registered under Scoped to allow this always-updated feature.
            if (lifeTime == ServiceLifetime.Singleton)
                services.AddSingleton<TSettingsType>(sp => sp.GetRequiredService<IOptions<TSettingsType>>().Value);
            else
                services.AddScoped<TSettingsType>(sp => sp.GetRequiredService<IOptionsSnapshot<TSettingsType>>().Value);

            return services;
        }

        /// <summary>
        /// Register a custom class based on the configuration section <paramref name="configurationSection"/>.
        /// </summary>
        /// <typeparam name="TSettingsType">Class that represents the configuration section.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configurationSection">Retuired specific section from the established configuration on the running application.</param>
        /// <param name="required">Whether or not the configuration section is a required section in the configuration.</param>
        /// <param name="settings">Out bound parameter that holds the custom, bound settings class of type <typeparamref name="TSettingsType"/> loaded from configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfigurationSettings<TSettingsType>(
            this IServiceCollection services,
            IConfigurationSection configurationSection,
            bool required,
            out TSettingsType settings)
            where TSettingsType : class, new()
        {
            AddCustomConfigurationSettings<TSettingsType>(services, 
                                                          configurationSection, 
                                                          required: required, 
                                                          lifeTime: ServiceLifetime.Singleton);

            settings = new TSettingsType();

            // NOTE: this exists check may not be necessary 
            //       doing it here because the "required" option may be set to false
            //       and not sure if there should be a check before calling "bind"
            if (configurationSection != null && configurationSection.Exists())
                configurationSection.Bind(settings);

            return services;
        }

        /// <summary>
        /// Register a custom class that represents a given configuration section on the <paramref name="configuration"/> instance.
        /// </summary>
        /// <typeparam name="TSettingsInterface">Interface that represents the configuration section.</typeparam>
        /// <typeparam name="TSettingsType">Class that represents the configuration section.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configuration">Established configuration on the running application.</param>
        /// <param name="sectionName">Name of the section found in <paramref name="configuration"/> that will be applied to <typeparamref name="TSettingsType"/>.</param>
        /// <param name="required">Whether or not the section indicated by <paramref name="sectionName"/> is a required section in the configuration. Defaults to true.</param>
        /// <param name="lifeTime">Registration lifetime. Defaults to singleton. If not singleton, configuration will reload when changed.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfigurationSettings<TSettingsInterface, TSettingsType>(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName,
            bool required = true,
            ServiceLifetime lifeTime = ServiceLifetime.Singleton)
            where TSettingsInterface : class
            where TSettingsType : class, TSettingsInterface, new()
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(configuration, nameof(configuration));

            var section = configuration.GetSection(sectionName, required: required);

            return AddCustomConfigurationSettings<TSettingsInterface, TSettingsType>(services, 
                                                                                     section,
                                                                                     required: required, 
                                                                                     lifeTime: lifeTime);
        }

        /// <summary>
        /// Register a custom class that represents a given configuration section on the <paramref name="configuration"/> instance.
        /// </summary>
        /// <typeparam name="TSettingsInterface">Interface that represents the configuration section.</typeparam>
        /// <typeparam name="TSettingsType">Class that represents the configuration section.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configuration">Established configuration on the running application.</param>
        /// <param name="sectionName">Name of the section found in <paramref name="configuration"/> that will be applied to <typeparamref name="TSettingsType"/>.</param>
        /// <param name="required">Whether or not the section is a required section in the configuration.</param>
        /// <param name="settings">Out bound parameter that holds the custom, bound settings class of type <typeparamref name="TSettingsType"/> loaded from configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfigurationSettings<TSettingsInterface, TSettingsType>(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName,
            bool required,
            out TSettingsType settings)
            where TSettingsInterface : class
            where TSettingsType : class, TSettingsInterface, new()
        {
            AddCustomConfigurationSettings<TSettingsInterface, TSettingsType>(services, 
                                                                              configuration, 
                                                                              sectionName, 
                                                                              required: required, 
                                                                              lifeTime: ServiceLifetime.Singleton);

            settings = new TSettingsType();

            // NOTE: this exists check may not be necessary 
            //       doing it here because the "required" option may be set to false
            //       and not sure if there should be a check before calling "bind"
            if (configuration.SectionExists(sectionName))
                configuration.Bind(sectionName, settings);

            return services;
        }

        /// <summary>
        /// Register a custom class based on the configuration section <paramref name="configurationSection"/>.
        /// </summary>
        /// <typeparam name="TSettingsInterface">Interface that represents the configuration section.</typeparam>
        /// <typeparam name="TSettingsType">Class that represents the configuration section.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configurationSection">Retuired specific section from the established configuration on the running application.</param>
        /// <param name="required">Whether or not the section is a required section in the configuration. Defaults to true.</param>
        /// <param name="lifeTime">Registration lifetime. Defaults to singleton. If not singleton, configuration will reload when changed.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfigurationSettings<TSettingsInterface, TSettingsType>(
            this IServiceCollection services,
            IConfigurationSection configurationSection,
            bool required = true,
            ServiceLifetime lifeTime = ServiceLifetime.Singleton)
            where TSettingsInterface : class
            where TSettingsType : class, TSettingsInterface, new()
        {
            Guard.IsNotNull(services, nameof(services));

            if (configurationSection == null || !configurationSection.Exists())
            {
                if (required)
                    throw new ConfigurationSectionException($"Required configuration section '{configurationSection?.Key}' was not found.");

                services.Add(new ServiceDescriptor(typeof(TSettingsType), typeof(TSettingsType), lifeTime));
                services.Add(new ServiceDescriptor(typeof(TSettingsInterface), typeof(TSettingsType), lifeTime));
                return services;
            }

            services.Configure<TSettingsType>(configurationSection);

            // Register as singleton or scoped. 
            // Singleton is one instance for the app. 
            // Scoped settings will be read on every request (no app restart on setting changes at the drawback of performance)
            // IOptionsSnapshot is also registered under Scoped to allow this always-updated feature.
            if (lifeTime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<TSettingsType>(sp => sp.GetRequiredService<IOptions<TSettingsType>>().Value);
                services.AddSingleton<TSettingsInterface>(sp => sp.GetRequiredService<TSettingsType>());
            }
            else
            {
                services.AddScoped<TSettingsType>(sp => sp.GetRequiredService<IOptionsSnapshot<TSettingsType>>().Value);
                services.AddScoped<TSettingsInterface>(sp => sp.GetRequiredService<TSettingsType>());
            }
                
            return services;
        }

        /// <summary>
        /// Register a custom class based on the configuration section <paramref name="configurationSection"/>.
        /// </summary>
        /// <typeparam name="TSettingsInterface">Interface that represents the configuration section.</typeparam>
        /// <typeparam name="TSettingsType">Class that represents the configuration section.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configurationSection">Retuired specific section from the established configuration on the running application.</param>
        /// <param name="required">Whether or not the section is a required section in the configuration.</param>
        /// <param name="settings">Out bound parameter that holds the custom, bound settings class of type <typeparamref name="TSettingsType"/> loaded from configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfigurationSettings<TSettingsInterface, TSettingsType>(
            this IServiceCollection services,
            IConfigurationSection configurationSection,
            bool required,
            out TSettingsType settings)
            where TSettingsInterface : class
            where TSettingsType : class, TSettingsInterface, new()
        {
            AddCustomConfigurationSettings<TSettingsInterface, TSettingsType>(services, 
                                                                              configurationSection, 
                                                                              required: required, 
                                                                              lifeTime: ServiceLifetime.Singleton);

            settings = new TSettingsType();

            // NOTE: this exists check may not be necessary 
            //       doing it here because the "required" option may be set to false
            //       and not sure if there should be a check before calling "bind"
            if (configurationSection != null && configurationSection.Exists())
                configurationSection.Bind(settings);

            return services;
        }

        /// <summary>
        /// Register <see cref="ISettings"/> to represent the standard "appsettings" using <see cref="ConfigurationAppSettings"/>.
        /// The default section name for application settings, "appsettings" or <see cref="ConfigurationAppSettings.DefaultSectionName"/> is required.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddAppSettings(this IServiceCollection services)
        {
            Guard.IsNotNull(services, nameof(services));

            services.TryAddSingleton<IValueParser, ValueParser>();
            services.TryAddSingleton<ISettings, ConfigurationAppSettings>();

            return services;
        }

        /// <summary>
        /// Register <see cref="ISettings"/> to represent the standard "appsettings" using <see cref="ConfigurationAppSettings"/>.
        /// The default section name for application settings, "appsettings" or <see cref="ConfigurationAppSettings.DefaultSectionName"/> is required.
        /// Allows for custom class <typeparamref name="TAppSettings"/> to represent application settings.
        /// </summary>
        /// <typeparam name="TAppSettings">Custom application settings class that should utilize values from appsettings and work with <see cref="ISettings"/>, probably under an adapter pattern.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddAppSettings<TAppSettings>(this IServiceCollection services)
            where TAppSettings : class, ISettings
        {
            Guard.IsNotNull(services, nameof(services));

            AddAppSettings(services);

            services.TryAddSingleton<TAppSettings>();

            return services;
        }

        /// <summary>
        /// Get registered configuration instance from the service provider.
        /// </summary>
        /// <param name="services">Existing service provider.</param>
        /// <returns></returns>
        public static IConfiguration GetConfiguration(this IServiceProvider services)
        {
            Guard.IsNotNull(services, nameof(services));
            return services.GetRequiredService<IConfiguration>();
        }
    }
}