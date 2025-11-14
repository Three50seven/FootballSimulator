using Common.Core.Domain;
using Common.Core.Validation;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Common.Core
{
    public static class EnumExtensions
    {
        private static ConcurrentDictionary<Tuple<Enum, string>, string> _friendlyNameLocalizedLookup = new ConcurrentDictionary<Tuple<Enum, string>, string>();

        //
        // Summary:
        //     Cache the Enum friendly name lookup values when using NetTango.Core.EnumExtensions.AsFriendlyName(System.Enum,System.String)
        //     from the executing assembly. Optionally provide a filter against all assemblies
        //     that will be in the current domain. Only call this function once - each call
        //     will reset the cache.
        //
        // Parameters:
        //   assemblyQuery:
        //     Query to filter upon all assemblies in the domain (i.e. - filter on the application's
        //     assemblies only). If null, the executing assembly will be used. NOTE: Be sure
        //     to provide a strong filter here to prevent overloading unnecessary enum values
        //     into the cache.
        public static void LoadFriendlyNames(Func<Assembly, bool>? assemblyQuery = null)
        {
            if (_friendlyNameLocalizedLookup == null || _friendlyNameLocalizedLookup.Count == 0)
            {
                _friendlyNameLocalizedLookup = new ConcurrentDictionary<Tuple<Enum, string>, string>();
            }

            Assembly[] array = ((assemblyQuery != null) ? AppDomain.CurrentDomain.GetAssemblies().FilterCustomAssemblies(assemblyQuery).ToArray() : new Assembly[1] { Assembly.GetExecutingAssembly() });
            string twoLetterISOLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            for (int i = 0; i < array.Length; i++)
            {
                Type[] array2 = (from t in array[i].GetTypes()
                                 where t.IsEnum
                                 select t).ToArray();
                for (int j = 0; j < array2.Length; j++)
                {
                    AddEnumTypeValuesToCache(array2[j], twoLetterISOLanguageName);
                }
            }
        }

        private static void AddEnumTypeValuesToCache(Type type, string culture)
        {
            Guard.IsNotNull(type, nameof(type));
            Array values = Enum.GetValues(type);
            for (int i = 0; i < values.Length; i++)
            {
                if (values.GetValue(i) is not Enum @enum)
                {
                    continue; // Skip null values to avoid CS8604 and CS8600
                }
                string? name = Enum.GetName(type, @enum);
                string value = @enum.ToString();
                if (name != null)
                {
                    FieldInfo? field = type.GetField(name);
                    if (field != null)
                    {
                        DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
                        if (array != null && array.Length != 0 && !string.IsNullOrWhiteSpace(array[0].Description))
                        {
                            value = ((array[0] is not LocalizedDescriptionAttribute) ? array[0].Description : (array[0] as LocalizedDescriptionAttribute)!.GetLocalizedDescription(new CultureInfo(culture)));
                        }
                    }
                }

                _friendlyNameLocalizedLookup.TryAdd(new Tuple<Enum, string>(@enum, culture), value);
            }
        }

        //
        // Summary:
        //     Return friendly string for this enum value. By default, the value as a string
        //     is returned. If the enum value is decorated with the System.ComponentModel.DescriptionAttribute,
        //     then that Description value will be returned. A static cache is used in this
        //     call and the enum type's other values will also be cached.
        //
        // Parameters:
        //   value:
        //     Application enum value.
        //
        //   culture:
        //     Optional localization culture to use for determining friendly name. Defaults
        //     to current UI culture.
        //
        // Returns:
        //     Friendly string either value itself or System.ComponentModel.DescriptionAttribute.Description
        //     on the value.
        public static string AsFriendlyName(this Enum value, string? culture = null)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            }

            if (_friendlyNameLocalizedLookup.TryGetValue(new Tuple<Enum, string>(value, culture), out var value2))
            {
                return value2;
            }

            Type type = value.GetType();
            AddEnumTypeValuesToCache(type, culture);
            if (!_friendlyNameLocalizedLookup.TryGetValue(new Tuple<Enum, string>(value, culture), out value2))
            {
                throw new InvalidEnumArgumentException($"Friendly name for enum {type.FullName} was not applied to cache or returned empty for value '{value}'.");
            }

            return value2;
        }

        //
        // Summary:
        //     Convert an enum value directly to an int using System.Convert.ToInt32(System.Object).
        //     Please consider casting this enum value to an int directly rather than calling
        //     this extension method.
        //
        // Parameters:
        //   enumValue:
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        //
        // Summary:
        //     Convert an Enum type to a list of select items.
        //
        // Parameters:
        //   enumType:
        //     Type that must be type System.Enum.
        //
        //   nameAsValue:
        //     Whether the name of the enum should be used as the select item value. If true,
        //     the name will be used, otherwise the Int/Id equivalant is used. Default is false.
        public static IEnumerable<SelectItem> ToSelectItemList(this Type enumType, bool nameAsValue = false)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("Type must be of type " + typeof(Enum).FullName + ".");
            }

            Array values = Enum.GetValues(enumType);
            foreach (Enum e in values)
            {
                yield return nameAsValue ? new SelectItem(e.ToString(), e.AsFriendlyName()) : new SelectItem(e);
            }
        }
    }
}
