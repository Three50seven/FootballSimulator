using Common.AspNetCore;
using Common.Logging.NLog;
using NLog.Web;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FootballSimulator.Web
{
    internal static class StartupLoggingConfiguration
    {
        [SupportedOSPlatform("windows")]
        public static ILoggingBuilder Configure(
            this ILoggingBuilder loggingBuilder,
            IConfiguration configuration)
        {
            loggingBuilder.ClearProviders();

#if DEBUG
            loggingBuilder.AddConsole()
                           .AddDebug()
                           .AddEventSourceLogger();
#endif

            loggingBuilder.AddNLogWeb(configuration.EstablishNLog());

            // include windows Event Log for errors
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                loggingBuilder.AddEventLog(settings =>
                {
                    settings.Filter = (err, level) => level >= LogLevel.Error;
                });
            }

            return loggingBuilder;
        }

        public static ExceptionLoggingOptions Options => new ExceptionLoggingOptions()
        {
            ShouldLogException = (httpContext, ex) =>
            {
                if (ex == null)
                    return false;
                if (ex is System.IO.IOException && (ex.Message?.Contains("An existing connection was forcibly closed by the remote host") ?? false))
                    return false;

                return true;
            }
        };
    }
}