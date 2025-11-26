using System;

namespace Common.Configuration
{
    /// <summary>
    /// Exception with Configuration Sections. Typically thrown if a specific section is not found.
    /// </summary>
    [Serializable]
    public class ConfigurationSectionException : Exception
    {
        public ConfigurationSectionException(string message) : base(message) { }
        public ConfigurationSectionException(string message, Exception inner) : base(message, inner) { }
    }
}
