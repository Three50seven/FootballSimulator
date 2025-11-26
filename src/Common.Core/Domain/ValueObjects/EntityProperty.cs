using Common.Core.Validation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Common.Core.Domain
{
    public class EntityProperty : ValueObject<EntityProperty>
    {
        protected EntityProperty () { }

        public EntityProperty(PropertyInfo property)
        {
            Guard.IsNotNull(property, nameof(property));

            Name = property.Name;
            Type = property.PropertyType.Name;

            var displayName = property.GetCustomAttributes<DisplayNameAttribute>(inherit: true).FirstOrDefault();
            FriendlyName = displayName == null ? Name : displayName.DisplayName;
        }

        public EntityProperty(string name, Type type, string friendlyName = null)
        {
            Name = name.SetEmptyToNull() ?? throw new ArgumentNullException(nameof(name));
            Type = type?.Name ?? throw new ArgumentNullException(nameof(type));
            FriendlyName = string.IsNullOrWhiteSpace(friendlyName) ? Name : friendlyName.Trim();
        }

        public EntityProperty(string name, string type, string friendlyName = null)
        {
            Name = name.SetEmptyToNull() ?? throw new ArgumentNullException(nameof(name));
            Type = type.SetEmptyToNull() ?? throw new ArgumentNullException(nameof(type));
            FriendlyName = string.IsNullOrWhiteSpace(friendlyName) ? Name : friendlyName.Trim();
        }

        public string Name { get; private set; }
        public string FriendlyName { get; private set; }
        public string Type { get; private set; }

        public static EntityProperty Empty => new EntityProperty();
    }
}
