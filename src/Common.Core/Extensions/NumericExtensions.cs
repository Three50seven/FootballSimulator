using Common.Core.Domain;

namespace Common.Core
{
    public static class NumericExtensions
    {
        /// <summary>
        /// Formats decimal in readable format.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="includeDecimalPlaces"></param>
        /// <returns></returns>
        public static string Format(this decimal? value, bool includeDecimalPlaces = false)
        {
            if (value == null)
                return "0";

            return ((decimal)value).Format(includeDecimalPlaces);
        }

        /// <summary>
        /// Formats decimal in readable format.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="includeDecimalPlaces"></param>
        /// <returns></returns>
        public static string Format(this decimal value, bool includeDecimalPlaces = false)
        {
            var s = string.Format("{0:0.00}", value);
            if (s.EndsWith("00") && !includeDecimalPlaces)
                return ((int)value).ToString();
            else
                return s;
        }

        /// <summary>
        /// Converts a 0 to null. Also converts negative numbers by default to null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="positiveOnly"></param>
        /// <returns></returns>
        public static int? CleanForNull(this int? value, bool positiveOnly = true)
        {
            if (value == null || value == 0 || (value < 0 && positiveOnly))
                return null;
            else
                return value;
        }

        /// <summary>
        /// Converts a 0 to null. Also converts negative numbers by default to null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="positiveOnly"></param>
        /// <returns></returns>
        public static decimal? CleanForNull(this decimal? value, bool positiveOnly = true)
        {
            if (value == null || value == 0 || (value < 0 && positiveOnly))
                return null;
            else
                return value;
        }

        /// <summary>
        /// Converts a 0 to null. Also converts negative numbers by default to null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="positiveOnly"></param>
        /// <returns></returns>
        public static double? CleanForNull(this double? value, bool positiveOnly = true)
        {
            if (value == null || value == 0 || (value < 0 && positiveOnly))
                return null;
            else
                return value;
        }

        /// <summary>
        /// Converts a 0 to null. Also converts negative numbers by default to null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="positiveOnly"></param>
        /// <returns></returns>
        public static long? CleanForNull(this long? value, bool positiveOnly = true)
        {
            if (value == null || value == 0 || (value < 0 && positiveOnly))
                return null;
            else
                return value;
        }

        /// <summary>
        /// Returns the proper suffix value. Returns "th", "st", "nd", or "rd"
        /// </summary>
        /// <param name="integer"></param>
        /// <returns></returns>
        public static string ToOccurrenceSuffix(this int integer)
        {
            switch (integer % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }
            switch (integer % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        /// <summary>
        /// Convert integer list to SelectItem list with Id values set to int values.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<SelectItem> ToSelectList(this IEnumerable<int> list)
        {
            return list?.Select(x => new SelectItem(x));
        }

        /// <summary>
        /// Get list of all pair combinations from list of ints
        /// Ref - http://stackoverflow.com/a/7242116
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, int>> ToPairsList(this IEnumerable<int> list)
        {
            if (list == null)
                return null;

            return (from item1 in list
                    from item2 in list
                    where item1 < item2
                    select Tuple.Create(item1, item2));
        }

        /// <summary>
        /// Perform divide operation.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divideBy"></param>
        /// <param name="roundUp"></param>
        /// <returns></returns>
        public static int DivideBy(this int value, int divideBy, bool roundUp = true)
        {
            int quotient = Math.DivRem(value, divideBy, out int remainder);
            return remainder == 0 || !roundUp ? quotient : quotient + 1;
        }
    }
}
