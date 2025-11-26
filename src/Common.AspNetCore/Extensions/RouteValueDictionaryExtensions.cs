using Microsoft.AspNetCore.Routing;

namespace Common.AspNetCore
{
    public static class RouteValueDictionaryExtensions
    {
        /// <summary>
        /// Adds new or updates existing values found in <paramref name="dest"/>.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static RouteValueDictionary Extend(this RouteValueDictionary dest, IEnumerable<KeyValuePair<string, object>> src)
        {
            foreach (var item in src)
            {
                dest.AddOrUpdate(item.Key, item.Value);
            }

            return dest;
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary source, IEnumerable<KeyValuePair<string, object>> newData)
        {
            return (new RouteValueDictionary(source)).Extend(newData);
        }

        public static void AddOrUpdate(this RouteValueDictionary source, string key, object value)
        {
            if (source == null || string.IsNullOrWhiteSpace(key))
                return;

            if (source.Any(x => x.Key.Contains(key)))
                source[key] = value;
            else
                source.Add(key, value);
        }

        /// <summary>
        /// Manually adds and returns collection of attributes needed for client-side (JQuery) validation.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static RouteValueDictionary SetAsRequiredValidation(this RouteValueDictionary attributes, string message)
        {
            string elementId;
            if (attributes.TryGetValue("id", out object id))
                elementId = id.ToString();
            else
                elementId = string.Empty;

            attributes.Add("data-val", "true");
            attributes.Add("data-val-required", message);
            attributes.Add("aria-required", "true");
            attributes.Add("aria-invalid", "false");
            attributes.Add("aria-describedby", $"{elementId}-error");

            return attributes;
        }
    }
}
