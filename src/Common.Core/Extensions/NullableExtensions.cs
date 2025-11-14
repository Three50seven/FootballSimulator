namespace Common.Core
{
    public static class NullableExtensions
    {
        //
        // Summary:
        //     Returns true if guid value is not null or Empty.
        //
        // Parameters:
        //   guid:
        public static bool HasValue(this Guid? guid)
        {
            return guid.HasValue && guid != Guid.Empty;
        }

        //
        // Summary:
        //     Returns true if date value is not null and not min or max date value.
        //
        // Parameters:
        //   date:
        public static bool HasValue(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return false;
            }

            return date.Value.HasValue();
        }

        //
        // Summary:
        //     Returns true if date value is not min or max date value.
        //
        // Parameters:
        //   date:
        public static bool HasValue(this DateTime date)
        {
            return date != DateTime.MinValue && date != DateTime.MaxValue;
        }
    }
}
