using System;
using System.Globalization;
using System.Linq;

namespace Common.Core
{
    public static class DateTimeHelper
    {
        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    throw new ArgumentException("Month value not a valid int; requires 1-12");
            }
        }

        public static string[] GetMonthNames()
        {
            return DateTimeFormatInfo.InvariantInfo.MonthNames.Take(12).ToArray();
        }
    }
}
