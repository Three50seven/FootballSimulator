using Common.Core.Domain;

namespace Common.Core
{
    public static class NumericExtensions
    {
        //
        // Summary:
        //     Formats decimal in readable format.
        //
        // Parameters:
        //   value:
        //
        //   includeDecimalPlaces:
        public static string Format(this decimal? value, bool includeDecimalPlaces = false)
        {
            if (!value.HasValue)
            {
                return "0";
            }

            return value.Value.Format(includeDecimalPlaces);
        }

        //
        // Summary:
        //     Formats decimal in readable format.
        //
        // Parameters:
        //   value:
        //
        //   includeDecimalPlaces:
        public static string Format(this decimal value, bool includeDecimalPlaces = false)
        {
            string text = $"{value:0.00}";
            if (text.EndsWith("00") && !includeDecimalPlaces)
            {
                return ((int)value).ToString();
            }

            return text;
        }

        //
        // Summary:
        //     Converts a 0 to null. Also converts negative numbers by default to null.
        //
        // Parameters:
        //   value:
        //
        //   positiveOnly:
        public static int? CleanForNull(this int? value, bool positiveOnly = true)
        {
            if (!value.HasValue || value == 0 || (value < 0 && positiveOnly))
            {
                return null;
            }

            return value;
        }

        //
        // Summary:
        //     Converts a 0 to null. Also converts negative numbers by default to null.
        //
        // Parameters:
        //   value:
        //
        //   positiveOnly:
        public static decimal? CleanForNull(this decimal? value, bool positiveOnly = true)
        {
            if (value.HasValue)
            {
                decimal? num = value;
                if (!((num.GetValueOrDefault() == default(decimal)) & num.HasValue))
                {
                    num = value;
                    if (!(((num.GetValueOrDefault() < default(decimal)) & num.HasValue) && positiveOnly))
                    {
                        return value;
                    }
                }
            }

            return null;
        }

        //
        // Summary:
        //     Converts a 0 to null. Also converts negative numbers by default to null.
        //
        // Parameters:
        //   value:
        //
        //   positiveOnly:
        public static double? CleanForNull(this double? value, bool positiveOnly = true)
        {
            if (!value.HasValue || value == 0.0 || (value < 0.0 && positiveOnly))
            {
                return null;
            }

            return value;
        }

        //
        // Summary:
        //     Converts a 0 to null. Also converts negative numbers by default to null.
        //
        // Parameters:
        //   value:
        //
        //   positiveOnly:
        public static long? CleanForNull(this long? value, bool positiveOnly = true)
        {
            if (!value.HasValue || value == 0 || (value < 0 && positiveOnly))
            {
                return null;
            }

            return value;
        }

        //
        // Summary:
        //     Returns the proper suffix value. Returns "th", "st", "nd", or "rd"
        //
        // Parameters:
        //   integer:
        public static string ToOccurrenceSuffix(this int integer)
        {
            int num = integer % 100;
            int num2 = num;
            if ((uint)(num2 - 11) <= 2u)
            {
                return "th";
            }

            return (integer % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th",
            };
        }

        //
        // Summary:
        //     Convert integer list to SelectItem list with Id values set to int values.
        //
        // Parameters:
        //   list:
        public static IEnumerable<SelectItem> ToSelectList(this IEnumerable<int> list)
        {
            return (list ?? Enumerable.Empty<int>()).Select(x => new SelectItem(x));
        }

        //
        // Summary:
        //     Get list of all pair combinations from list of ints Ref - http://stackoverflow.com/a/7242116
        //
        //
        // Parameters:
        //   list:
        public static IEnumerable<Tuple<int, int>>? ToPairsList(this IEnumerable<int> list)
        {
            if (list == null)
            {
                return null;
            }

            return from item1 in list
                   from item2 in list
                   where item1 < item2
                   select Tuple.Create(item1, item2);
        }

        //
        // Summary:
        //     Perform divide operation.
        //
        // Parameters:
        //   value:
        //
        //   divideBy:
        //
        //   roundUp:
        public static int DivideBy(this int value, int divideBy, bool roundUp = true)
        {
            int result;
            int num = Math.DivRem(value, divideBy, out result);
            return (result == 0 || !roundUp) ? num : (num + 1);
        }
    }
}
