using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;

namespace Common.Logging.NLog
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Establish NLog loggers and factories with the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="loggingConfiguration">Required configuration for NLog.</param>
        /// <param name="useServiceProviderFactory">
        /// Whether or not NLog's services should attempt to use the service provider to resolve classes. 
        /// Defaults to false. Benefits to this approach are yet to really surface, but it's available to try.</param>
        /// <param name="minLevel">Min level to set for logging.</param>
        /// <returns></returns>
        public static IServiceCollection AddNLogLogging(
            this IServiceCollection services,
            LoggingConfiguration loggingConfiguration,
            bool useServiceProviderFactory = false,
            Microsoft.Extensions.Logging.LogLevel minLevel = Microsoft.Extensions.Logging.LogLevel.Information)
        {
            ArgumentNullException.ThrowIfNull(loggingConfiguration);

            // Set the global NLog configuration before adding NLog to the logging builder
            LogManager.Configuration = loggingConfiguration;

            services.TryAddSingleton<LoggingConfiguration>(loggingConfiguration);

            services.AddSingleton<ILoggerFactory, LoggerFactory>()
                    .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                    .AddLogging((builder) =>
                    {
                        builder.SetMinimumLevel(minLevel);

                        if (useServiceProviderFactory)
                        {
                            builder.AddNLog(); // No longer supports serviceProvider overload in NLog 5.x
                        }
                        else
                        {
                            builder.AddNLog();
                        }
                    });

            return services;
        }
    }
}
