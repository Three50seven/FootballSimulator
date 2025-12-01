using Common.Core;

namespace FootballSimulator.Web
{
    public static class WebExtensions
    {
        public static bool IsLocalUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            // Check if the URL starts with a single slash (/) but not a double slash (//) or a backslash (\)
            if (url.StartsWith("/") && !url.StartsWith("//") && !url.StartsWith(@"\"))
            {
                return true;
            }
            // Check if the URL starts with a tilde followed by a slash (~/)
            if (url.StartsWith("~/"))
            {
                return true;
            }

            return false;
        }
    }
}
