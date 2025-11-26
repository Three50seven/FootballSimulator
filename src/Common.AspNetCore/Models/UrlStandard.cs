using Common.Core;
using System;

namespace Common.AspNetCore
{
    public static class UrlStandard
    {
        public const string DefaultRelativePath = "/";

        public static string VerifyAsUrl(string url)
        {
            return VerifyAsUrl(url, DefaultRelativePath);
        }

        public static string VerifyAsUrl(string url, string fallbackUrl)
        {
            if (string.IsNullOrWhiteSpace(fallbackUrl))
                fallbackUrl = DefaultRelativePath;

            if (string.IsNullOrWhiteSpace(url))
                return fallbackUrl;

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                return uri.PathAndQuery;
            else if (Uri.TryCreate(url, UriKind.Relative, out uri))
                return uri.ToString();
            else
                return fallbackUrl;
        }

        public static bool FriendlyNameMatches(string friendlyName, string name)
        {
            if (string.IsNullOrWhiteSpace(friendlyName) || string.IsNullOrWhiteSpace(name))
                return false;

            return friendlyName.Equals(name.ToUrlFriendlyString());
        }
    }
}
