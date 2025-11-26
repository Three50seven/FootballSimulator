namespace Common.Core
{
    public static class DateTimeFormats
    {
        public const string FullDateTime = "yyyy-MM-ddTHH:mm:ss.fffZ";
        public const string DateAndTime = "MM/dd/yyyy h:mm:ss tt";
        public const string DateAndShortTime = "MM/dd/yyyy h:mm tt";
        public const string ShortDate = "MM/dd/yy";
        public const string ShortDateFullYear = "MM/dd/yyyy";
        public const string FullWeekdayMonthDateTime = "ddd, MMMM d, yyyy - h:mm:ss tt"; //e.g. Thur, April 6, 2017 - 10:50:58 AM
        public const string LongMonthDate = "MMMM d, yyyy"; //e.g. April 6, 2017
    }
}
