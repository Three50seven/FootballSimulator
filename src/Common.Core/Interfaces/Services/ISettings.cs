namespace Common.Core
{
    public interface ISettings
    {
        T Get<T>(string key, bool required = true, T defaultValue = default(T));
#if FEATURE_DEFAULT_INTERFACE_METHODS
        string Get(string key, bool required = true, string defaultValue = null)
        {
            return Get<string>(key, required, defaultValue);
        }
#else
        string Get(string key, bool required = true, string defaultValue = null);
#endif
    }
}
