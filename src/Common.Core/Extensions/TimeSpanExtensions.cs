using System;
using System.Text;

namespace Common.Core
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Returns friendly text of the timespan. Example: 3 days, 7 hrs, 1 min
        /// </summary>
        /// <param name="time"></param>
        /// <param name="includeSeconds">Whether or not to include seconds.</param>
        /// <returns></returns>
        public static string ToDurationString(this TimeSpan time, bool includeSeconds = true)
        {
            var sb = new StringBuilder();

            if (time.Days > 0)
                sb.Append(string.Format("{0} {1}, ", time.Days, time.Days > 1 ? "days" : "day"));
                
            if (time.Hours > 0)
                sb.Append(string.Format("{0} {1}, ", time.Hours, time.Hours > 1 ? "hrs" : "hr"));

            if (time.Minutes > 0)
                sb.Append(string.Format("{0} {1}, ", time.Minutes, time.Minutes > 1 ? "mins" : "min"));

            if (includeSeconds && time.Seconds > 0)
                sb.Append(string.Format("{0} {1}, ", time.Seconds, time.Seconds > 1 ? "seconds" : "second"));

            if (sb.Length > 0)
                sb.Remove(sb.ToString().LastIndexOf(","), 1);

            return sb.ToString();
        }
    }
}
