using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Common.AspNetCore
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Clear response declarations/headers of cache values. The follow headers are set to not cache or are removed:
        /// <see cref="HeaderNames.CacheControl"/>, <see cref="HeaderNames.Pragma"/>, <see cref="HeaderNames.Expires"/>, and <see cref="HeaderNames.ETag"/>.
        /// </summary>
        /// <typeparam name="THttpResponse"></typeparam>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        public static THttpResponse ClearCache<THttpResponse>(this THttpResponse httpResponse)
            where THttpResponse : HttpResponse
        {
            httpResponse.Headers[HeaderNames.CacheControl] = "no-cache";
            httpResponse.Headers[HeaderNames.Pragma] = "no-cache";
            httpResponse.Headers[HeaderNames.Expires] = "-1";
            httpResponse.Headers.Remove(HeaderNames.ETag);

            return httpResponse;
        }
    }
}
