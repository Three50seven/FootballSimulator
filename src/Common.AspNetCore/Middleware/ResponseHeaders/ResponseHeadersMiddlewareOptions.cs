using System.Collections.Generic;

namespace Common.AspNetCore
{
    public class ResponseHeadersMiddlewareOptions
    {
        public ResponseHeadersMiddlewareOptions(
            string[] headersToRemove, 
            KeyValuePair<string, string>[] headersToAdd)
        {
            HeadersToRemove = headersToRemove ?? [];
            HeadersToAdd = headersToAdd ?? [];
        }

        public string[] HeadersToRemove { get; private set; }
        public KeyValuePair<string, string>[] HeadersToAdd { get; private set; }

        public bool AdjustResponseHeaders => HeadersToRemove.Length > 0 || HeadersToAdd.Length > 0;
    }
}
