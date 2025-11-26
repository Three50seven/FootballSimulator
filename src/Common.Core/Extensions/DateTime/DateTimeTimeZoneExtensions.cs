using System;

namespace Common.Core
{
    public static class DateTimeTimeZoneExtensions
    {
        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, string timeZoneId = "Eastern Standard Time")
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return ToTimeZoneTime(time, tzi);
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, timeZone);
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime? ToTimeZoneTime(this DateTime? time)
        {
            if (time == null)
                return null;

            return ToTimeZoneTime((DateTime)time);
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static DateTime? ToTimeZoneTime(this DateTime? time, TimeZoneInfo timeZone)
        {
            if (time == null)
                return null;

            return ToTimeZoneTime((DateTime)time, timeZone);
        }
    }
}
