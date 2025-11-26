using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace Common.AspNetCore.Mvc
{
    public static class ModelMetaDataExtensions
    {
        public static object[] GetCustomAttributes(this ModelMetadata metaData)
        {
            return GetCustomAttributes(metaData, null);
        }

        public static object[] GetCustomAttributes(this ModelMetadata metaData, Type type)
        {
            if (metaData == null)
                return null;

            var prop = metaData.ContainerType.GetProperty(metaData.PropertyName);

            if (type == null)
                return prop.GetCustomAttributes(false);
            else
                return prop.GetCustomAttributes(type, false);
        }

        public static bool HasCustomAttribute<T>(this ModelMetadata metaData)
        {
            return HasCustomAttribute(metaData, typeof(T));
        }

        public static bool HasCustomAttribute(this ModelMetadata metaData, Type type)
        {
            if (metaData == null || metaData.ContainerType == null)
                return false;

            var property = metaData.ContainerType.GetProperty(metaData.PropertyName);
            return property != null && property.GetCustomAttributes(type, true).Length != 0;
        }
    }
}
