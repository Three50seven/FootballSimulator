using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;

namespace Common.Logging.NLog
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Read configuration values for "NLog" from the provided configuration.
        /// The NLog configuration is then applied to any custom Targets and LayoutRenders and <see cref="LogManager.Configuration"/>.
        /// </summary>
        /// <param name="configuration">Established configuration for running application.</param>
        /// <param name="sectionName">Name of section of configuration for NLog configuration.</param>
        /// <param name="initTargetsCallback">Optional callback to add any custom targets prior to configuration being parsed.</param>
        /// <param name="configurationCallback">Optional callback to build upon the NLog configuration.</param>
        /// <returns></returns>
        public static NLogLoggingConfiguration EstablishNLog(
            this IConfiguration configuration,
            string sectionName = "NLog",
            Action<IConfiguration>? initTargetsCallback = null,
            Action<IConfiguration, NLogLoggingConfiguration>? configurationCallback = null)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            // set static configuration value so the connection string can be read from configuration
            ConnectionStringLayoutRenderer.Configuration = configuration;

            initTargetsCallback?.Invoke(configuration);

            var nlogConfiguration = new NLogLoggingConfiguration(configuration.GetSection(sectionName ?? "NLog"));

            // set some default custom properties
            GlobalDiagnosticsContext.Set("appbasepath", System.IO.Directory.GetCurrentDirectory());

            configurationCallback?.Invoke(configuration, nlogConfiguration);

            LogManager.Configuration = nlogConfiguration;

            return nlogConfiguration;
        }
    }
}
