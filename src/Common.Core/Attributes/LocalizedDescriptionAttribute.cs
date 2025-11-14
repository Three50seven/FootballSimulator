using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace Common.Core.Domain
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _key;

        private readonly ResourceManager _resourceManager;

        public override string Description => Resolve(_resourceManager.GetString(_key) ?? string.Empty);

        public LocalizedDescriptionAttribute(string key, Type resourceType)
        {
            _resourceManager = new ResourceManager(resourceType);
            _key = key;
        }

        protected virtual string Resolve(string resourceValue)
        {
            return string.IsNullOrEmpty(resourceValue) ? $"[[{_key}]]" : resourceValue;
        }

        //
        // Summary:
        //     Pull NetTango.Core.LocalizedDescriptionAttribute.Description from localized resource.
        //
        //
        // Parameters:
        //   culture:
        public virtual string GetLocalizedDescription(CultureInfo culture)
        {
            var resourceValue = _resourceManager.GetString(_key, culture) ?? string.Empty;
            return Resolve(resourceValue);
        }
    }
}
