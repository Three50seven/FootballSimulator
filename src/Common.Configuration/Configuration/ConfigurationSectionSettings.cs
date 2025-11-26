using Microsoft.Extensions.Configuration;
using Common.Core;
using Common.Core.Validation;

namespace Common.Configuration
{
    /// <summary>
    /// Implementation of <see cref="ISettings"/> that queries for values within a custom Section (by name) on a <see cref="IConfiguration"/> instance.
    /// </summary>
    public class ConfigurationSectionSettings : ConfigurationSettings
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="valueParser"></param>
        /// <param name="configuration">Configuration instance that contains a section under the supplied name.</param>
        /// <param name="sectionName">Section name found in the <see cref="IConfiguration"/> instance.</param>
        public ConfigurationSectionSettings(IValueParser valueParser, IConfiguration configuration, string sectionName)
            : base (valueParser, configuration)
        {
            Guard.IsNotNull(sectionName, nameof(sectionName));

            SectionName = sectionName;
        }

        public string SectionName { get; private set; }

        protected override bool TryGetBaseValue(string key, out string value)
        {
            return base.TryGetBaseValue($"{SectionName}:{key}", out value);
        }
    }
}
