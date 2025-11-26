using System;

namespace Common.Core
{
    public static class DateTimeFormatExtensions
    {
        public static string DefaultFormat = DateTimeFormats.ShortDateFullYear;

        /// <summary>
        /// Format a date value into a custom, readable  stringformat.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format">Format on which to output the date. Defaults to <see cref="DefaultFormat"/>.</param>
        /// <param name="useExtendedSpecifiers">Wehter to check for nn and NN for custom occurance text.</param>
        /// <returns></returns>
        public static string Format(
            this DateTime? date, 
            string format = null, 
            bool useExtendedSpecifiers = false)
        {
            if (!date.HasValue())
                return string.Empty;
            else
                return ((DateTime)date).Format(format, useExtendedSpecifiers);
        }

        /// <summary>
        /// Format a date value into a custom, readable  stringformat.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format">Format on which to output the date. Defaults to <see cref="DefaultFormat"/>.</param>
        /// <param name="useExtendedSpecifiers">Wehter to check for nn and NN for custom occurance text.</param>
        /// <returns></returns>
        public static string Format(
            this DateTime date, 
            string format = null, 
            bool useExtendedSpecifiers = false)
        {
            if (string.IsNullOrWhiteSpace(format))
                format = DefaultFormat;

            var formattedDate = date.ToString(format);

            if (useExtendedSpecifiers)
            {
                return formattedDate
                    .Replace("nn", date.Day.ToOccurrenceSuffix().ToLower())
                    .Replace("NN", date.Day.ToOccurrenceSuffix().ToUpper());
            }
            else
                return formattedDate;
        }
    }
}
