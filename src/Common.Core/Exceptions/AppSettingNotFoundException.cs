using System;

namespace Common.Core
{
    [Serializable]
    public class AppSettingNotFoundException : Exception
    {
        public AppSettingNotFoundException() { }
        public AppSettingNotFoundException(string key) 
            : base($"The application setting with key \"{key}\" was not found.".Sanitize()) { }

        public AppSettingNotFoundException(string key, Exception inner) 
            : base($"The application setting with key \"{key}\" was not found.".Sanitize(), inner) { }
    }
}
