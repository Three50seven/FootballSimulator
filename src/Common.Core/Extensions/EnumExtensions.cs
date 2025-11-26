using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Common.Core
{
    public static class EnumExtensions
    {
        private static ConcurrentDictionary<Tuple<Enum, string>, string> _friendlyNameLocalizedLookup = new ConcurrentDictionary<Tuple<Enum, string>, string>();

        /// <summary>
        /// Cache the Enum friendly name lookup values when using <see cref="AsFriendlyName(Enum, string)"></see> from the executing assembly.
        /// Optionally provide a filter against all assemblies that will be in the current domain.
        /// Only call this function once - each call will reset the cache.
        /// </summary>
        /// <param name="assemblyQuery">
        /// Query to filter upon all assemblies in the domain (i.e. - filter on the application's assemblies only). If null, the executing assembly will be used.
        /// NOTE: Be sure to provide a strong filter here to prevent overloading unnecessary enum values into the cache.
        /// </param>
        public static void LoadFriendlyNames(Func<Assembly, bool> assemblyQuery = null)
        {
            if (_friendlyNameLocalizedLookup == null || _friendlyNameLocalizedLookup.Count == 0)
                _friendlyNameLocalizedLookup = new ConcurrentDictionary<Tuple<Enum, string>, string>();

            Assembly[] assemblies;

            if (assemblyQuery == null)
            {
                assemblies = new Assembly[] { Assembly.GetExecutingAssembly() };
            }
            else
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                                    .FilterCustomAssemblies(assemblyQuery)
                                                    .ToArray();
            }

            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            for (int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes().Where(t => t.IsEnum).ToArray();
                for (int t = 0; t < types.Length; t++)
                {
                    AddEnumTypeValuesToCache(types[t], culture);
                }
            }
        }

        private static void AddEnumTypeValuesToCache(Type type, string culture)
        {
            Guard.IsNotNull(type, nameof(type));

            var values = Enum.GetValues(type);

            for (int v = 0; v < values.Length; v++)
            {
                var enumValue = values.GetValue(v) as Enum;
                string name = Enum.GetName(type, enumValue);
                string friendlyName = enumValue.ToString();

                if (name != null)
                {
                    FieldInfo field = type.GetField(name);
                    if (field != null)
                    {
                        var attributes = (DescriptionAttribute[])field.GetCustomAttributes(
                                                                    typeof(DescriptionAttribute),
                                                                    false);

                        if (attributes != null && attributes.Length > 0 && !string.IsNullOrWhiteSpace(attributes[0].Description))
                        {
                            if (attributes[0] is LocalizedDescriptionAttribute)
                                friendlyName = (attributes[0] as LocalizedDescriptionAttribute).GetLocalizedDescription(new CultureInfo(culture));
                            else
                                friendlyName = attributes[0].Description;
                        }
                    }
                }

                _friendlyNameLocalizedLookup.TryAdd(new Tuple<Enum, string>(enumValue, culture), friendlyName);
            }
        }

        /// <summary>
        /// Return friendly string for this enum value. By default, the value as a string is returned.
        /// If the enum value is decorated with the <see cref="DescriptionAttribute"/>, then that Description value will be returned.
        /// A static cache is used in this call and the enum type's other values will also be cached.
        /// </summary>
        /// <param name="value">Application enum value.</param>
        /// <param name="culture">Optional localization culture to use for determining friendly name. Defaults to current UI culture.</param>
        /// <returns>Friendly string either value itself or <see cref="DescriptionAttribute.Description"/> on the value.</returns>
        public static string AsFriendlyName(this Enum value, string culture = null)
        {
            if (value == null)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(culture))
                culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            // return from cache
            if (_friendlyNameLocalizedLookup.TryGetValue(new Tuple<Enum, string>(value, culture), out string friendlyName))
                return friendlyName;

            // load all enum values from that type and into the cache
            var type = value.GetType();
            AddEnumTypeValuesToCache(type, culture);

            // try again - it should be in the cache now
            if (!_friendlyNameLocalizedLookup.TryGetValue(new Tuple<Enum, string>(value, culture), out friendlyName))
                throw new InvalidEnumArgumentException($"Friendly name for enum {type.FullName} was not applied to cache or returned empty for value '{value}'.");

            return friendlyName;
        }

        /// <summary>
        /// Convert an enum value directly to an int using <see cref="Convert.ToInt32(object)"/>.
        /// Please consider casting this enum value to an int directly rather than calling this extension method.
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        /// <summary>
        /// Convert an Enum type to a list of select items.
        /// </summary>
        /// <param name="enumType">Type that must be type <see cref="Enum"/>.</param>
        /// <param name="nameAsValue">
        /// Whether the name of the enum should be used as the select item value. 
        /// If true, the name will be used, otherwise the Int/Id equivalant is used. 
        /// Default is false.
        /// </param>
        /// <returns></returns>
        public static IEnumerable<SelectItem> ToSelectItemList(this Type enumType, bool nameAsValue = false)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException($"Type must be of type {typeof(Enum).FullName}.");

            Array values = Enum.GetValues(enumType);

            foreach (Enum e in values)
            {
                yield return nameAsValue ? new SelectItem(e.ToString(), AsFriendlyName(e)) : new SelectItem(e);
            }
        }
    }
}
