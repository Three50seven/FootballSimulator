using Common.Core.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Whether the provided type has a public, parameterless constructor.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasDefaultConstructor(this Type type)
        {
            return type?.GetConstructor(Type.EmptyTypes) != null;
        }

        /// <summary>
        /// Whether or not the type is considered a "collection" or "list" or "array" type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCollectionType(this Type type)
        {
            return (from interfaceType in type.GetInterfaces()
                    where interfaceType.IsGenericType
                    let baseInterface = interfaceType.GetGenericTypeDefinition()
                    where (baseInterface == typeof(IEnumerable<>)) || (baseInterface == typeof(IEnumerable))
                    select interfaceType).Any()
                    ||
                    typeof(IEnumerable).IsAssignableFrom(type) || typeof(IEnumerable<>).IsAssignableFrom(type)
                    ||
                    (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                    );
        }

        /// <summary>
        /// Determine whether a type is simple (String, Decimal, DateTime, etc) 
        /// or complex (i.e. custom class with public properties and methods).
        /// Ref: http://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive
        /// </summary>
        public static bool IsSimpleType(this Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                new Type[] {
                    typeof(String),
                    typeof(Decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }

        /// <summary>
        /// Get list of the interfaces that are applied to a given <see cref="Type">Type</see> <paramref name="type"/>.
        /// Only the top-level interfaces will be included, which includes the interfaces applied to the class itself.
        /// If <paramref name="includeAbstractBaseInterface"/> is true, and the class inherits from an abstract base class
        /// that includes an interface, that interface will be included. Only a single level of inheritance is examined for this logic.
        /// </summary>
        /// <param name="type">Class type.</param>
        /// <param name="includeAbstractBaseInterface">Include interfaces on abstract base class if present. Defaults to true.</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTopLevelInterfaces(this Type type, bool includeAbstractBaseInterface = true)
        {
            if (type == null)
                return null;

            var allInterfaces = type.GetInterfaces();

            // distinct list from any sub interfaces
            var interfaces = allInterfaces
                .Where(x => !allInterfaces.Any(y => y.GetInterfaces().Contains(x)));

            // remove any interfaces also found on the base type as long as base is not abstract
            // NOTE: only doing one level of inheritance to keep it simple
            if (type.BaseType != null && (!type.BaseType.IsAbstract || !includeAbstractBaseInterface))
                interfaces = interfaces.Except(type.BaseType.GetInterfaces());

            return interfaces;
        }

        public static bool IsPropertyArrayOrList(this Type type)
        {
            if (type == null)
                return false;

            return type.IsArray && type.GetElementType() == typeof(string) ||
                (type != typeof(string) && type.GetInterface(typeof(IEnumerable<>).FullName) != null);
        }

        /// <summary>
        /// Returns if type is a class via <see cref="Type.IsClass"/> and the type's full name does not start with "System"
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsUserDefinedClass(this Type type)
        {
            if (type == null)
                return false;

            return type.IsClass && !type.FullName.StartsWith("System.");
        }

        /// <summary>
        /// Return list of custom attributes of the supplied type, <typeparamref name="T"/>, if applied on type <paramref name="type"/>.
        /// Calls into <see cref="System.Reflection.MemberInfo.GetCustomAttributes(Type, bool)"/>.
        /// </summary>
        /// <typeparam name="T">Attribute type.</typeparam>
        /// <param name="type">Reference type where attribute is applied</param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit = false)
            where T : Attribute
        {
            Guard.IsNotNull(type, nameof(type));

            return type.GetCustomAttributes(typeof(T), inherit).Select(attr => (T)attr);
        }

        /// <summary>
        /// Check a type for any Attributes of type <typeparamref name="T"/>. Returns true if attribute is present, otherwise false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <param name="inherit"></param>
        /// <returns>Returns true if attribute is present, otherwise false.</returns>
        public static bool TryGetAttribute<T>(this Type type, out T attribute, bool inherit = false)
            where T : Attribute
        {
            Guard.IsNotNull(type, nameof(type));

            var attributes = type.GetCustomAttributes(typeof(T), inherit);

            if (attributes == null || attributes.Length == 0)
            {
                attribute = null;
                return false;
            }

            attribute = (T)attributes.First();
            return attribute != null;
        }
    }
}
