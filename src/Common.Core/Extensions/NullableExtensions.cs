using System;

namespace Common.Core
{
    public static class NullableExtensions
    {
        /// <summary>
        /// Returns true if guid value is not null or Empty.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool HasValue(this Guid? guid)
        {
            return guid != null && guid != Guid.Empty;
        }

        /// <summary>
        /// Returns true if date value is not null and not min or max date value.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool HasValue(this DateTime? date)
        {
            if (date == null)
                return false;
            else
                return ((DateTime)date).HasValue();
        }

        /// <summary>
        /// Returns true if date value is not min or max date value.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool HasValue(this DateTime date)
        {
            return date != DateTime.MinValue && date != DateTime.MaxValue;
        }
    }
}
