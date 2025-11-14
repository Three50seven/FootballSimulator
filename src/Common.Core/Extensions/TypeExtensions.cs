using Common.Core.Validation;
using System.Collections;

namespace Common.Core
{
    public static class TypeExtensions
    {
        //
        // Summary:
        //     Whether the provided type has a public, parameterless constructor.
        //
        // Parameters:
        //   type:
        public static bool HasDefaultConstructor(this Type type)
        {
            return type?.GetConstructor(Type.EmptyTypes) != null;
        }

        //
        // Summary:
        //     Whether or not the type is considered a "collection" or "list" or "array" type.
        //
        //
        // Parameters:
        //   type:
        public static bool IsCollectionType(this Type type)
        {
            return (from interfaceType in type.GetInterfaces()
                    where interfaceType.IsGenericType
                    let baseInterface = interfaceType.GetGenericTypeDefinition()
                    where baseInterface == typeof(IEnumerable<>) || baseInterface == typeof(IEnumerable)
                    select interfaceType).Any() || typeof(IEnumerable).IsAssignableFrom(type) || typeof(IEnumerable<>).IsAssignableFrom(type) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        //
        // Summary:
        //     Determine whether a type is simple (String, Decimal, DateTime, etc) or complex
        //     (i.e. custom class with public properties and methods). Ref: http://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive
        public static bool IsSimpleType(this Type type)
        {
            return type.IsValueType || type.IsPrimitive || new Type[6]
            {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
            }.Contains(type) || Convert.GetTypeCode(type) != TypeCode.Object;
        }

        //
        // Summary:
        //     Get list of the interfaces that are applied to a given Type type. Only the top-level
        //     interfaces will be included, which includes the interfaces applied to the class
        //     itself. If includeAbstractBaseInterface is true, and the class inherits from
        //     an abstract base class that includes an interface, that interface will be included.
        //     Only a single level of inheritance is examined for this logic.
        //
        // Parameters:
        //   type:
        //     Class type.
        //
        //   includeAbstractBaseInterface:
        //     Include interfaces on abstract base class if present. Defaults to true.
        public static IEnumerable<Type>? GetTopLevelInterfaces(this Type type, bool includeAbstractBaseInterface = true)
        {
            if (type == null)
            {
                return null;
            }

            Type[] allInterfaces = type.GetInterfaces();
            IEnumerable<Type> enumerable = allInterfaces.Where((Type x) => !allInterfaces.Any((Type y) => y.GetInterfaces().Contains(x)));
            if (type.BaseType != null && (!type.BaseType.IsAbstract || !includeAbstractBaseInterface))
            {
                enumerable = enumerable.Except(type.BaseType.GetInterfaces());
            }

            return enumerable;
        }

        public static bool IsPropertyArrayOrList(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            // Fix CS8604 by ensuring FullName is not null before calling GetInterface
            string? ienumerableGenericFullName = typeof(IEnumerable<>).FullName;
            return (type.IsArray && type.GetElementType() == typeof(string)) ||
                   (type != typeof(string) && ienumerableGenericFullName != null && type.GetInterface(ienumerableGenericFullName) != null);
        }

        //
        // Summary:
        //     Returns if type is a class via System.Type.IsClass and the type's full name does
        //     not start with "System"
        //
        // Parameters:
        //   type:
        public static bool IsUserDefinedClass(this Type type)
        {
            if (type == null || type.FullName == null)
            {
                return false;
            }

            return type.IsClass && !type.FullName.StartsWith("System.");
        }

        //
        // Summary:
        //     Return list of custom attributes of the supplied type, T, if applied on type
        //     type. Calls into System.Reflection.MemberInfo.GetCustomAttributes(System.Type,System.Boolean).
        //
        //
        // Parameters:
        //   type:
        //     Reference type where attribute is applied
        //
        //   inherit:
        //
        // Type parameters:
        //   T:
        //     Attribute type.
        public static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit = false) where T : Attribute
        {
            Guard.IsNotNull(type, nameof(type));
            return from attr in type.GetCustomAttributes(typeof(T), inherit)
                   select (T)attr;
        }

        //
        // Summary:
        //     Check a type for any Attributes of type T. Returns true if attribute is present,
        //     otherwise false.
        //
        // Parameters:
        //   type:
        //
        //   attribute:
        //
        //   inherit:
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     Returns true if attribute is present, otherwise false.
        public static bool TryGetAttribute<T>(this Type type, out T? attribute, bool inherit = false) where T : Attribute
        {
            Guard.IsNotNull(type, nameof(type));
            object[] customAttributes = type.GetCustomAttributes(typeof(T), inherit);
            if (customAttributes == null || customAttributes.Length == 0)
            {
                attribute = null;
                return false;
            }

            attribute = (T)customAttributes.First();
            return attribute != null;
        }
    }
}
