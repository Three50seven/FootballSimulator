using Microsoft.Extensions.Configuration;
using Common.Core;

namespace Common.Configuration
{
    /// <summary>
    /// Implementation of <see cref="ISettings"/> that queries for values within standard section named <see cref="DefaultSectionName"/> on a <see cref="IConfiguration"/> instance.
    /// Considered to be the default implmentation for <see cref="ISettings"/> as it represents original ASP.NET's configuration lookup of key-value pairs in appSettings configuration section.
    /// </summary>
    public class ConfigurationAppSettings : ConfigurationSectionSettings
    {
        public const string DefaultSectionName = "appsettings";

        public ConfigurationAppSettings(IValueParser valueParser, IConfiguration configuration)
            : base (valueParser, configuration, DefaultSectionName)
        {

        }
    }
}