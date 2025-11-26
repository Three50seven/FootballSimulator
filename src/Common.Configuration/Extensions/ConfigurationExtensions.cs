using Microsoft.Extensions.Configuration;
using Common.Core.Validation;

namespace Common.Configuration
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Get section from configuration. Optionally force the section to be required.
        /// </summary>
        /// <param name="configuration">Established configuration.</param>
        /// <param name="name">Name of the section. "AppSettings", "ConnectionStrings", etc. </param>
        /// <param name="required">Whether the section is required to exist in the configuration.</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationSectionException"></exception>
        public static IConfigurationSection GetSection(this IConfiguration configuration, string name, bool required)
        {
            Guard.IsNotNull(configuration, nameof(configuration));
            Guard.IsNotNull(name, nameof(name));

            var section = configuration.GetSection(name);
            if (!required)
                return section;

            if (section == null || !section.Exists())
                throw new ConfigurationSectionException($"Configuration section '{name}' was not found.");

            return section;
        }

        /// <summary>
        /// Determines is a section with key/name of <paramref name="name"/> is present on the configuration.
        /// </summary>
        /// <param name="configuration">Established configuration.</param>
        /// <param name="name">Name of the section. "AppSettings", "ConnectionStrings", etc.</param>
        /// <returns></returns>
        public static bool SectionExists(this IConfiguration configuration, string name)
        {
            Guard.IsNotNull(configuration, nameof(configuration));
            Guard.IsNotNull(name, nameof(name));

            var section = configuration.GetSection(name);
            return section != null && section.Exists();
        }
    }
}
