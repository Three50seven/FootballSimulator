using Common.Core.Validation;

namespace Common.Core.Services
{
    public abstract class SettingsBase : ISettings
    {
        public SettingsBase(IValueParser parser)
        {
            Parser = parser;
        }

        protected IValueParser Parser { get; private set; }

#if !FEATURE_DEFAULT_INTERFACE_METHODS
        public virtual string Get(string key, bool required = true, string defaultValue = null)
        {
            return Get<string>(key, required, defaultValue);
        }
#endif
        public virtual T Get<T>(string key, bool required = true, T defaultValue = default(T))
        {
            Guard.IsNotNull(key, nameof(key));

            if (!TryGetBaseValue(key, out string value))
            {
                if (required)
                    throw new AppSettingNotFoundException(key);
                else
                    return defaultValue;
            }

            return Parser.Parse<T>(value);
        }

        protected abstract bool TryGetBaseValue(string key, out string value);
    }
}
