using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace Common.Core
{
    /// <summary>
    /// Description attribute that will use <see cref="ResourceManager"/> to pull <see cref="Description"/> from resources.
    /// </summary>
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _key;
        private readonly ResourceManager _resourceManager;

        public LocalizedDescriptionAttribute(string key, Type resourceType)
        {
            _resourceManager = new ResourceManager(resourceType);
            _key = key;
        }

        public override string Description
        {
            get
            {
                return Resolve(_resourceManager.GetString(_key)!);
            }
        }

        protected virtual string Resolve(string resourceValue)
        {
            return string.IsNullOrEmpty(resourceValue)
                   ? string.Format("[[{0}]]", _key)
                   : resourceValue;
        }

        /// <summary>
        /// Pull <see cref="Description"/> from localized resource.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual string GetLocalizedDescription(CultureInfo culture)
        {
            return Resolve(_resourceManager.GetString(_key, culture)!);
        }
    }
}
