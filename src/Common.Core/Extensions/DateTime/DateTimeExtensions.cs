using Common.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Return day one / first day of the month and year of the given date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToFirstDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Return the last day (28th, 29th, 30th, or 31st) of the month and year of the given date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToLastDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        /// <summary>
        /// Get the date of the Monday of the current week designated by the supplied date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToFirstDateOfWeek(this DateTime date)
        {
            int dayIndex = 0;
            int dayOfMonth = date.Day; //grab the current date's day of month

            //current weekday index; to help calculate number between days
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dayIndex = 0;
                    break;
                case DayOfWeek.Tuesday:
                    dayIndex = 1;
                    break;
                case DayOfWeek.Wednesday:
                    dayIndex = 2;
                    break;
                case DayOfWeek.Thursday:
                    dayIndex = 3;
                    break;
                case DayOfWeek.Friday:
                    dayIndex = 4;
                    break;
                case DayOfWeek.Saturday:
                    dayIndex = 5;
                    break;
                case DayOfWeek.Sunday:
                    dayIndex = 6;
                    break;
            }

            //default year and month values to the current
            int year = date.Year;
            int month = date.Month;
            int newDayOfMonth = 0;

            //check to see if the current weekday index is over the day of month (the week is split across 2 months)
            if (dayIndex >= dayOfMonth)
            {
                //it's janurary (the week also splits across 2 years)
                if (date.Month == 1)
                {
                    month = 12; //previous month is dec
                    year--; //decrement the year
                }
                else
                    month--; //just decrement the month to the previous month of the current year

                var numOfDaysInMonth = DateTime.DaysInMonth(year, month); //get the number of days in the month
                newDayOfMonth = numOfDaysInMonth - (dayIndex - dayOfMonth); //calculate the date needed by counting back from current date and then previous month's end date
            }
            else
                newDayOfMonth = dayOfMonth - dayIndex; //simply grab the difference to get Monday's date (which is within the current month)

            return new DateTime(year, month, newDayOfMonth);
        }

        /// <summary>
        /// Return the date of the first Monday that appears in the month of the supplied date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToFirstMondayOfMonth(this DateTime date)
        {
            var monday = new DateTime(date.Year, date.Month, 1);
            if (monday.DayOfWeek != DayOfWeek.Monday)
            {
                do { monday = monday.AddDays(1); }
                while (monday.DayOfWeek != DayOfWeek.Monday);
            }

            return monday;
        }

        /// <summary>
        /// Whether or not the time of the supplied date is midnight. 
        /// This also means the time of the DateTime object was not set.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsMidnight(this DateTime? date)
        {
            if (!date.HasValue())
                return false;
            return IsMidnight((DateTime)date);
        }

        /// <summary>
        /// Whether or not the time of the supplied date is midnight. 
        /// This also means the time of the DateTime object was not set.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsMidnight(this DateTime date)
        {
            if (!date.HasValue())
                return false;
            return date.TimeOfDay == TimeSpan.Zero;
        }


        /// <summary>
        /// Returns the date of the first Monday of the year of the supplied date.
        /// Optionally supply a custom year in order to run the calculation. 
        /// Otherwise, the year of the supplied date is used.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="year">Optional year to run the calculation.</param>
        /// <returns></returns>
        public static DateTime ToFirstMondayOfYear(this DateTime date, int? year = null)
        {
            int day = 0;
            if (year == null)
                year = date.Year;

            while ((new DateTime((int)year, 01, ++day)).DayOfWeek != DayOfWeek.Monday) ;
            return new DateTime((int)year, 01, day);
        }

        /// <summary>
        /// Return the number of years back the supplied date is from today.
        /// </summary>
        /// <param name="dateOfBirth"></param>
        /// <returns></returns>
        public static int YearsOld(this DateTime dateOfBirth)
        {
            if (!dateOfBirth.HasValue())
                return 0;

            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age -= 1;

            return age;
        }

        /// <summary>
        /// Return Age value object based on the today and the supplied date.
        /// </summary>
        /// <param name="dateOfBirth"></param>
        /// <returns></returns>
        public static Age ToAge(this DateTime dateOfBirth)
        {
            return new Age(dateOfBirth);
        }

        /// <summary>
        /// Return the date of the weekday <paramref name="dayOfWeek"/> in the week of the supplied date.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime ToSpecificDayOfCurrentWeek(this DateTime date, DayOfWeek dayOfWeek)
        {
            if (date == DateTime.MinValue || date == DateTime.MaxValue)
                return date;

            if (date.DayOfWeek == dayOfWeek)
                return date;

            // go forward in days if current day of week is before target day of week
            // else go back in days to target
            int incrementer = date.DayOfWeek < dayOfWeek ? 1 : -1;

            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(incrementer);
            }

            return date;
        }

        /// <summary>
        /// Get a SelectItem represented by the datetime value.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="valueFormat">Optional format for the Value for the datetime. Defaults to ToShortDateString().</param>
        /// <param name="nameFormat">Optional format for the Name for the datetime. Defaults to ToShortDateString().</param>
        /// <returns></returns>
        public static SelectItem ToSelectItem(
            this DateTime date, 
            string valueFormat = null, 
            string nameFormat = null)
        {
            if (!date.HasValue())
                return null;

            return new SelectItem(
                string.IsNullOrWhiteSpace(valueFormat) ? date.ToShortDateString() : date.Format(valueFormat),
                string.IsNullOrWhiteSpace(nameFormat) ? date.ToShortDateString() : date.Format(nameFormat));
        }

        /// <summary>
        /// Select list of SelectItem represented by the datetime value.
        /// </summary>
        /// <param name="dates"></param>
        /// <param name="valueFormat">Optional format for the Value for the datetime. Defaults to ToShortDateString().</param>
        /// <param name="nameFormat">Optional format for the Name for the datetime. Defaults to ToShortDateString().</param>
        /// <returns></returns>
        public static IEnumerable<SelectItem> ToSelectItems(
            this IEnumerable<DateTime> dates,
            string valueFormat = null,
            string nameFormat = null)
        {
            return dates?.Select(date => date.ToSelectItem(valueFormat, nameFormat));
        }

        /// <summary>
        /// Select list of SelectItem represented by the datetime Year value.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static IEnumerable<SelectItem> ToYearSelectItems(this IEnumerable<DateTime> dates)
        {
            return dates?.Select(d => d.Year).Distinct().Select(year => new SelectItem(year));
        }

        public static int NumberOfMonths(this DateTime startDate, DateTime endDate)
        {
            return ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month + 1;
        }
    }
}
