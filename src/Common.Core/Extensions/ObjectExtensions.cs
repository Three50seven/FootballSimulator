using System.Collections;
using System.Reflection;

namespace Common.Core
{
    public static class ObjectExtensions
    {
        //
        // Summary:
        //     Uses reflection to determine if object has a property with name propertyName.
        //
        //
        // Parameters:
        //   obj:
        //
        //   propertyName:
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.Property(propertyName) != null;
        }

        //
        // Summary:
        //     Uses reflection to get property info directly from object.
        //
        // Parameters:
        //   obj:
        //
        //   propertyName:
        public static PropertyInfo? Property(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName);
        }

        //
        // Summary:
        //     Uses reflection to get list of collection properties available on a given type
        //     type. Reference - http://stackoverflow.com/questions/24598122/getting-all-icollection-properties-through-reflection
        //
        //
        // Parameters:
        //   type:
        public static IEnumerable<PropertyInfo> CollectionProperties(this Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            IEnumerable<PropertyInfo> first = from prop in properties
                                              from interfaceType in prop.PropertyType.GetInterfaces()
                                              where interfaceType.IsGenericType
                                              let baseInterface = interfaceType.GetGenericTypeDefinition()
                                              where baseInterface == typeof(ICollection<>) || baseInterface == typeof(ICollection)
                                              select prop;
            IEnumerable<PropertyInfo> second = properties.Where((PropertyInfo prop) => typeof(ICollection).IsAssignableFrom(prop.PropertyType) || typeof(ICollection<>).IsAssignableFrom(prop.PropertyType));
            IEnumerable<PropertyInfo> second2 = properties.Where((PropertyInfo prop) => prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>));
            return first.Union(second).Union(second2);
        }

        //
        // Summary:
        //     Uses reflection to get all properties based on the object type.
        //
        // Parameters:
        //   value:
        public static IEnumerable<PropertyInfo>? GetAllProperties(this object value)
        {
            return value?.GetType().GetProperties();
        }

        //
        // Summary:
        //     Uses reflection to lookup through all properites of a given type and call a callback
        //     function to apply a consistent event for all properties, including those found
        //     in collection properties.
        //
        // Parameters:
        //   model:
        //
        //   onGetNewValue:
        //
        // Type parameters:
        //   T:
        public static void ApplyToProperties<T>(this object model, Func<PropertyInfo, T, T> onGetNewValue)
        {
            if (model == null)
            {
                return;
            }

            if (model.GetType().IsCollectionType())
            {
                foreach (object item in (IEnumerable)model)
                {
                    item.ApplyToProperties(onGetNewValue);
                }

                return;
            }

            IEnumerable<PropertyInfo> allProperties = model.GetAllProperties()!;
            foreach (PropertyInfo item2 in allProperties)
            {
                ApplyToProperty(item2, model, onGetNewValue);
            }
        }

        private static void ApplyToProperty<T>(PropertyInfo propertyInfo, object model, Func<PropertyInfo, T, T> onGetNewValue)
        {
            try
            {
                if (propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.GetValue(model) != null)
                {
                    var propertyValue = propertyInfo.GetValue(model);
                    if ((propertyInfo.PropertyType.IsUserDefinedClass() || propertyInfo.PropertyType.IsCollectionType()) && propertyInfo.CanWrite)
                    {
                        if (propertyValue != null)
                        {
                            propertyValue.ApplyToProperties(onGetNewValue);
                        }
                    }
                    else if (propertyInfo.PropertyType == typeof(T) && propertyInfo.CanWrite)
                    {
                        T val = onGetNewValue(propertyInfo, (T)propertyValue!);
                        propertyInfo.SetValue(model, val);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
