using System;
using System.Net;

namespace Common.Core.Domain
{
    public static class RequestInfoExtensions
    {
        [Obsolete("This web request abstraction extension is now obsolete. Use HttpClient instance directly for sending web requests.")]
        public static HttpWebRequest ToHttpWebRequest(this RequestInfo requestInfo)
        {
            if (requestInfo == null)
                return null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestInfo.Action);
            request.Method = requestInfo.Method;
            request.ContentType = requestInfo.ContentType;
            request.Accept = requestInfo.ContentType;

            if (!string.IsNullOrWhiteSpace(requestInfo.Token))
            {
                request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {requestInfo.Token}");
            }

            if (!string.IsNullOrWhiteSpace(requestInfo.Token))
            {
                request.Headers.Add(HttpRequestHeader.Cookie, $"PD-S-SESSION-ID={requestInfo.Cookie}");
            }

            return request;
        }
    }
}
