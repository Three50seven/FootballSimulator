using Common.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core.Domain
{
    [Obsolete("This web request abstraction class is now obsolete. Use HttpClient instance directly for sending web requests.")]
    public class RequestInfo : ValueObject<RequestInfo>
    {
        public static ICollection<string> WhiteList = new HashSet<string>() { "http://localhost", "https://localhost" };

        public static string DefaultContentType = "application/json";
        public static string DefaultMethod = "GET";

        protected RequestInfo() { }

        public RequestInfo(
            string action, 
            object data = null, 
            string token = null, 
            string method = null, 
            string contentType = null,
            string cookie = null)
        {
            Guard.IsNotNull(action, nameof(action));

            Action = action.Trim();

            // Match the incoming URL against a whitelist
            if (!WhiteList.Any(w => Action.Contains(w)))
                throw new InvalidOperationException($"Action '{Action.Sanitize()}' not allowed. Add to static {nameof(RequestInfo.WhiteList)} to allow requests to this endpoint.");

            Data = data;
            Token = token.SetEmptyToNull();
            ContentType = string.IsNullOrWhiteSpace(contentType) ? DefaultContentType : contentType.ToLower().Trim();
            Method = string.IsNullOrWhiteSpace(method) ? DefaultMethod : method.ToUpper().Trim();
            Cookie = cookie.SetEmptyToNull();
        }

        public string Action { get; private set; }
        public string Method { get; private set; }
        public string ContentType { get; private set; }
        public object Data { get; private set; }
        public string Token { get; private set; }

        public string Cookie { get; private set; }

        public virtual bool ActionIsAbsolute
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Action))
                    return false;

                return Action.StartsWith("http");
            }
        }

        public static string BuildAbsoluteAction(string domain, string action)
        {
            domain = domain.SetNullToEmpty();
            action = action.SetNullToEmpty();

            if (string.IsNullOrWhiteSpace(action))
                return domain;
            if (string.IsNullOrWhiteSpace(domain))
                return action;

            if (action.StartsWith("~"))
                action = action.Substring(1);
            if (!action.StartsWith("/"))
                action = string.Concat("/", action);

            if (domain.EndsWith("/"))
                domain = domain.TrimEnd('/');

            return string.Concat(domain, action);
        }
    }
}
