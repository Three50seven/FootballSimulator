using Microsoft.AspNetCore.Mvc;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Remove cache properties and headers from response.
    /// </summary>
    public class NoResponseCacheAttribute : ResponseCacheAttribute
    {
        public NoResponseCacheAttribute()
        {
            Location = ResponseCacheLocation.None;
            NoStore = true;
            Duration = 0;
        }
    }
}
