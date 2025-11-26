using Microsoft.AspNetCore.Mvc.Rendering;
using Common.Core;
using System;
using System.Collections.Generic;

namespace Common.AspNetCore.Mvc
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Convert a Enum type <paramref name="enumType"/> options to a list of <see cref="SelectListItem"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="nameAsValue">Whether the value should be the same as the name. <see cref="EnumExtensions.AsFriendlyName(Enum, string)"/> is used for the Text in both cases.</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectList(this Type enumType, bool nameAsValue = false)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException($"Type must be of type {typeof(Enum).FullName}.");

            Array values = Enum.GetValues(enumType);

            foreach (Enum e in values)
            {
                yield return new SelectListItem()
                {
                    Text = e.AsFriendlyName(),
                    Value = nameAsValue ? e.AsFriendlyName() : e.ToInt().ToString()
                };
            }
        }
       
    }
}
