using Microsoft.AspNetCore.Mvc;
using Common.Core;
using System;

namespace Common.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string BaseUrl(this IUrlHelper helper)
        {
            return BaseUrl(helper, includeTrailingSlash: true);
        }

        public static string BaseUrl(this IUrlHelper helper, bool includeTrailingSlash)
        {
            ArgumentNullException.ThrowIfNull(helper);

            return helper.ActionContext.HttpContext.Request.GetBaseUrl(includeTrailingSlash);
        }

        public static string ActionSEO(this IUrlHelper helper, string action, Guid identifier, string name)
        {
            return helper.Action(action, new { @id = identifier, @name = name.ToUrlFriendlyString() });
        }

        public static string ActionSEO(this IUrlHelper helper, string action, string controller, Guid identifier, string name)
        {
            return helper.Action(action, controller, new { @id = identifier, @name = name.ToUrlFriendlyString() });
        }

        public static string AbsoluteAction(this IUrlHelper helper, string action, string controller)
        {
            return AbsoluteAction(helper, action, controller, null);
        }

        public static string AbsoluteAction(this IUrlHelper helper, string action, string controller, object routeValues)
        {
            return helper.Action(action, controller, routeValues, protocol: helper.ActionContext.HttpContext.Request.Scheme);
        }

        public static string AbsoluteUrl(this IUrlHelper helper, string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return null;

            relativeUrl = relativeUrl.SetNullToEmpty(true);

            if (relativeUrl.StartsWith('~'))
                relativeUrl = relativeUrl[1..];
            if (!relativeUrl.StartsWith('/'))
                relativeUrl = string.Concat("/", relativeUrl);

            var uri = new Uri(relativeUrl, UriKind.RelativeOrAbsolute);

            // if the URI is not already absolute, rebuild it based on the current request.
            if (!uri.IsAbsoluteUri)
                return uri.ToAbsoluteUrl(helper.ActionContext.HttpContext.Request.GetUri());
            else
                return uri.ToString();
        }
    }
}
