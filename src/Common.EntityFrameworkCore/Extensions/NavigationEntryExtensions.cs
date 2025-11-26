using Microsoft.EntityFrameworkCore.ChangeTracking;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public static class NavigationEntryExtensions
    {
        public static EntityProperty ToEntityProperty(this NavigationEntry entry, string name = null)
        {
            if (entry == null)
                return null;

            return string.IsNullOrWhiteSpace(name) ? new EntityProperty(entry.Metadata.PropertyInfo)
                : new EntityProperty(entry.Metadata.PropertyInfo.Name, entry.Metadata.PropertyInfo.PropertyType, name);
        }
    }
}
