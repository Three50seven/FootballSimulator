using System;

namespace Common.AspNetCore
{
    public static class UriExtensions
    {
        /// <summary>
        /// Return string url as a relative path if the uri is absolute.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string ToRelativeUrl(this Uri uri)
        {
            ArgumentNullException.ThrowIfNull(uri);

            return uri.IsAbsoluteUri ? uri.PathAndQuery : uri.OriginalString;
        }

        /// <summary>
        /// Attempts to return absolute url string of the uri based on the provided base.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this Uri uri, string baseUrl)
        {
            return ToAbsoluteUrl(uri, new Uri(baseUrl));
        }

        /// <summary>
        /// Attempts to return absolute url string of the uri based on the provided base.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this Uri uri, Uri baseUri)
        {
            ArgumentNullException.ThrowIfNull(uri);
            ArgumentNullException.ThrowIfNull(baseUri);

            if (Uri.TryCreate(baseUri, ToRelativeUrl(uri), out Uri absolute))
                return absolute.ToString();

            return uri.IsAbsoluteUri ? uri.ToString() : null;
        }
    }
}
