using System.Reflection;

namespace Common.Core
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Determines if a property's type is a collection.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsCollection(this PropertyInfo property)
        {
            return property.PropertyType.IsCollectionType();
        }
    }
}
