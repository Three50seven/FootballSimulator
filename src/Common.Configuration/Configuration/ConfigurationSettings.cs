using Microsoft.Extensions.Configuration;
using Common.Core;
using Common.Core.Services;

namespace Common.Configuration
{
    /// <summary>
    /// Implementation of <see cref="ISettings"/> that queries for values directly on a base <see cref="IConfiguration"/> instance.
    /// </summary>
    public class ConfigurationSettings : SettingsBase
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="valueParser"></param>
        /// <param name="configuration">Configuration instance to lookup values via key.</param>
        public ConfigurationSettings(IValueParser valueParser, IConfiguration configuration) 
            : base(valueParser)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; private set; }

        protected override bool TryGetBaseValue(string key, out string value)
        {
            if (key == null)
                throw new System.ArgumentNullException(nameof(key));

            if (Configuration == null)
                throw new System.InvalidOperationException("Configuration instance is not set.");

            value = Configuration[key] ?? string.Empty;

            return value != null;
        }
    }
}