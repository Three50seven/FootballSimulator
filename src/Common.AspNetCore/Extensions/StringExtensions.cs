namespace Common.AspNetCore
{
    public static class StringExtensions
    {
        /// <summary>
        /// Format a date format string <paramref name="value"/> for proper JavaScript format.
        /// Defaults to using formats accepted by MomentJS.
        /// </summary>
        /// <param name="value">Date/time format string.</param>
        /// <param name="forMomentJS">Whether custom format for MomentJS should be used.</param>
        /// <returns></returns>
        public static string ToJSFriendlyDateFormat(this string value, bool forMomentJS = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (forMomentJS)
                return MomentJSHelper.GenerateMomentJSFormatString(value);
            else
                return value.Replace('d', 'D').Replace('y', 'Y').Replace("tt", "a").Replace('t', 'a');
        }
    }
}
