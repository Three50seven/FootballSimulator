namespace Common.Core
{
    public static class BoolExtensions
    {
        /// <summary>
        /// Returns "1" for true, "0" for false.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSqlValueString(this bool value)
        {
            return ToString(value, "1", "0");
        }

        /// <summary>
        /// Returns "Yes" for true, "No" for false.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYesNoString(this bool? value)
        {
            if (value == null)
                return string.Empty;

            return ToYesNoString((bool)value);
        }

        /// <summary>
        /// Returns "Yes" for true, "No" for false.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYesNoString(this bool value)
        {
            return ToString(value, "Yes", "No");
        }

        /// <summary>
        /// Returns lower string version of bool.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLowerString(this bool value)
        {
            return value.ToString().ToLower();
        }

        /// <summary>
        /// Return custom string representation of the boolean value. Returns <paramref name="trueText"/> when true, <paramref name="falseText"/> when false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="trueText">Text when value is true.</param>
        /// <param name="falseText">Text when value is false.</param>
        /// <returns></returns>
        public static string ToString(this bool value, string trueText, string falseText)
        {
            return value ? trueText : falseText;
        }
    }
}
