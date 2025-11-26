using Microsoft.Extensions.Configuration;
using NLog;
using System;

namespace Common.Logging.NLog
{
    /// <summary>
    /// Simple disposable logger class using NLog to use during the immediate startup of AspNetCore and .NET Core applications, 
    /// typically inside Program.cs and before the service collection is established.
    /// Configuration will be parsed to establish NLog during the construction of this class.
    /// </summary>
    public class StartupLogger : IDisposable
    {
        private readonly Logger _logger;
        private readonly string _applicationName;

        public StartupLogger(
            IConfiguration config, 
            string applicationName = "Application",
            string loggerName = "StartupLogger")
        {
            config.EstablishNLog();
            _applicationName = applicationName;

            // create nlog logger for startup
            _logger = LogManager.GetLogger(loggerName);
        }

        public void Info(string message)
        {
            _logger.Info($"Startup for application '{_applicationName}' - {message}");
        }

        public void Error(Exception ex)
        {
            _logger.Error(ex, $"Error on startup for application '{_applicationName}'.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                LogManager.Shutdown();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StartupLogger()
        {
            Dispose(false);
        }
    }
}
