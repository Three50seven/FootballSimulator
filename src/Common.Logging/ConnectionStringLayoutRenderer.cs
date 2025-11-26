using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using System.Text;

namespace Common.Logging.NLog
{
    /// <summary>
    /// Custom layout renderer that returns the full connection string found in the ConnectionStrings section of appsettings.json.
    /// NLog (still) does not support the connection strings section of an appsettings.json file, requiring this custom solution.
    /// Be sure the public <see cref="ConnectionStringLayoutRenderer.Configuration"/> is set during start of application.
    /// </summary>
    [LayoutRenderer("connstring")]
    public class ConnectionStringLayoutRenderer : LayoutRenderer
    {
        public static IConfiguration Configuration { private get; set; }

        public ConnectionStringLayoutRenderer()
        {
            if (Configuration == null)
                throw new NLogConfigurationException($"Configuration not set for {GetType().FullName}. Set static Configuration property before use.");
        }

        [RequiredParameter]
        [DefaultParameter]
        public string Name { get; set; }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            if (string.IsNullOrWhiteSpace(Name) || Configuration == null)
                return;

            builder.Append(Configuration.GetConnectionString(Name));
        }
    }
}
