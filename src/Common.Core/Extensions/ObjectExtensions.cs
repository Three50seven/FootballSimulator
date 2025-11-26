using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Core
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Uses reflection to determine if object has a property with name <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this object obj, string propertyName)
        {
            return Property(obj, propertyName) != null;
        }

        /// <summary>
        /// Uses reflection to get property info directly from object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo Property(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName);
        }

        /// <summary>
        /// Uses reflection to get list of collection properties available on a given type <paramref name="type"/>.
        /// Reference - http://stackoverflow.com/questions/24598122/getting-all-icollection-properties-through-reflection
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> CollectionProperties(this Type type)
        {
            var properties = type.GetProperties();

            // Get properties with PropertyType declared as interface
            var interfaceProps =
                from prop in properties

                from interfaceType in prop.PropertyType.GetInterfaces()
                where interfaceType.IsGenericType
                let baseInterface = interfaceType.GetGenericTypeDefinition()
                where (baseInterface == typeof(ICollection<>)) || (baseInterface == typeof(ICollection))
                select prop;

            // Get properties with PropertyType declared(probably) as solid types.
            var nonInterfaceProps =
                from prop in properties
                where typeof(ICollection).IsAssignableFrom(prop.PropertyType) || typeof(ICollection<>).IsAssignableFrom(prop.PropertyType)
                select prop;

            // get generic type collections
            var genericTypes =
                from prop in properties
                where prop.PropertyType.IsGenericType
                    && prop.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
                select prop;

            // Combine  queries into one resulting
            return interfaceProps.Union(nonInterfaceProps).Union(genericTypes);
        }

        /// <summary>
        /// Uses reflection to get all properties based on the object type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetAllProperties(this object value)
        {
            if (value == null)
                return null;

            return value.GetType().GetProperties();
        }

        /// <summary>
        /// Uses reflection to lookup through all properites of a given type and call a callback function to apply 
        /// a consistent event for all properties, including those found in collection properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="onGetNewValue"></param>
        public static void ApplyToProperties<T>(this object model, Func<PropertyInfo, T, T> onGetNewValue)
        {
            if (model == null)
                return;

            if (model.GetType().IsCollectionType())
            {
                foreach (var item in (IEnumerable)model)
                {
                    ApplyToProperties(item, onGetNewValue);
                }
            }
            else
            {
                var properties = GetAllProperties(model);
                foreach (var property in properties)
                {
                    ApplyToProperty(property, model, onGetNewValue);
                }
            }
        }

        private static void ApplyToProperty<T>(PropertyInfo propertyInfo, object model, Func<PropertyInfo, T, T> onGetNewValue)
        {
            try
            {
                if (propertyInfo.GetIndexParameters().Length != 0 || propertyInfo.GetValue(model) == null)
                    return;

                if ((propertyInfo.PropertyType.IsUserDefinedClass() || propertyInfo.PropertyType.IsCollectionType()) && propertyInfo.CanWrite)
                {
                    ApplyToProperties(propertyInfo.GetValue(model), onGetNewValue);
                }
                else if (propertyInfo.PropertyType == typeof(T) && propertyInfo.CanWrite)
                {
                    var updatedValue = onGetNewValue.Invoke(propertyInfo, (T)propertyInfo.GetValue(model));
                    propertyInfo.SetValue(model, updatedValue);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
