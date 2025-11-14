using Common.Core.Validation;
using System.ComponentModel;
using System.Reflection;

namespace Common.Core.Domain
{
    public class EntityProperty : ValueObject<EntityProperty>
    {
        public string Name { get; private set; } = string.Empty;

        public string FriendlyName { get; private set; } = string.Empty;

        public string Type { get; private set; } = string.Empty;

        public static EntityProperty Empty => new EntityProperty();

        protected EntityProperty()
        {
        }

        public EntityProperty(PropertyInfo property)
        {
            Guard.IsNotNull(property, nameof(property));
            Name = property.Name;
            Type = property.PropertyType.Name;
            DisplayNameAttribute? displayNameAttribute = property.GetCustomAttributes<DisplayNameAttribute>(inherit: true).FirstOrDefault();
            FriendlyName = ((displayNameAttribute == null) ? Name : displayNameAttribute.DisplayName);
        }

        public EntityProperty(string name, Type type, string? friendlyName = null)
        {
            Name = name.SetEmptyToNull() ?? throw new ArgumentNullException(nameof(name));
            Type = type?.Name ?? throw new ArgumentNullException("type");
            FriendlyName = (string.IsNullOrWhiteSpace(friendlyName) ? Name : friendlyName.Trim());
        }

        public EntityProperty(string name, string type, string? friendlyName = null)
        {
            Name = name.SetEmptyToNull() ?? throw new ArgumentNullException("name");
            Type = type.SetEmptyToNull() ?? throw new ArgumentNullException("type");
            FriendlyName = (string.IsNullOrWhiteSpace(friendlyName) ? Name : friendlyName.Trim());
        }
    }
}
