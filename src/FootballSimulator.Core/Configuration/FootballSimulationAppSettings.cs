using Common.Core;

namespace FootballSimulator
{
    public class FootballSimulationAppSettings : ISettings
    {
        private readonly ISettings _settings;

        public FootballSimulationAppSettings(ISettings settings)
        {
            _settings = settings;

            MiniProfilerEnabled = Get<bool>(SettingKeys.MiniProfilerEnabled);
            DomainUrl = Get(SettingKeys.DomainUrl);
        }

        public bool MiniProfilerEnabled { get; }

        public string DomainUrl { get; }

        public string Get(string key, bool required = true, string? defaultValue = null)
        {
            return _settings.Get(key, required, defaultValue);
        }

        public T Get<T>(string key, bool required = true, T? defaultValue = default)
        {
            return _settings.Get(key, required, defaultValue);
        }
    }
}
