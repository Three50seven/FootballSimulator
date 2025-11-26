using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Common.Core.Validation;
using System;

namespace Common.AspNetCore
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Return whether the request was performed via an ajax call.
        /// This means the "X-Requested-With" header has a value of "XMLHttpRequest";
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjax(this HttpRequest request)
        {
            if (request == null)
                return false;

            return request.Headers.XRequestedWith == "XMLHttpRequest";
        }

        /// <summary>
        /// Return a new Uri based on the display url of the request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Uri GetUri(this HttpRequest request)
        {
            return new Uri(request.GetDisplayUrl());
        }

        /// <summary>
        /// Attempt to get the referring url. This attempts to read the "Referer" header in the request.
        /// If not found the default path <see cref="UrlStandard.DefaultRelativePath"/> is returned or
        /// the optional <paramref name="fallBackUrl"/> is returned if provided.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fallBackUrl">Optional fallback url to return if referring url is not found.</param>
        /// <returns></returns>
        public static string GetUrlReferrer(this HttpRequest request, string fallBackUrl = null)
        {
            Guard.IsNotNull(request, nameof(request));

            if (string.IsNullOrWhiteSpace(fallBackUrl))
                fallBackUrl = UrlStandard.DefaultRelativePath;

            if (!request.Headers.TryGetValue("Referer", out StringValues value))
                return fallBackUrl;

            return value.ToString().ToLower();
        }

        /// <summary>
        /// Attempt to get a valid url for the user to return to a favorable destination.
        /// Attempts to get referring url and verifies valid urls.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <param name="fallbackUrl"></param>
        /// <returns></returns>
        public static string GetReturnUrl(this HttpRequest request, string url = null, string fallbackUrl = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                url = GetUrlReferrer(request);

            if (string.IsNullOrWhiteSpace(fallbackUrl))
                fallbackUrl = UrlStandard.DefaultRelativePath;

            return UrlStandard.VerifyAsUrl(url, fallbackUrl);
        }

        /// <summary>
        /// Builds a url from the request's Uri by using the scheme and authority.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="includeTrailingSlash"></param>
        /// <returns></returns>
        public static string GetBaseUrl(this HttpRequest request, bool includeTrailingSlash = true)
        {
            Guard.IsNotNull(request, nameof(request));

            var uri = request.GetUri();
            return string.Concat(
                uri.Scheme,
                "://",
                uri.Authority,
                (includeTrailingSlash ? "/" : string.Empty));
        }
    }
}
