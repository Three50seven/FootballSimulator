using Common.Core.Validation;
using System;

namespace Common.Core.Domain
{
    public class DateTimeRange : ValueObject<DateTimeRange>
    {
        protected DateTimeRange() { }

        public DateTimeRange(DateTime startDate, DateTime endDate)
        {
            Guard.IsValidDateRange(startDate, endDate);

            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTimeRange(DateTime startDate, TimeSpan span)
            : this(startDate, startDate.Add(span))
        {
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        // determine if StartDate is before current date and EndDate is greater than current date
        public bool IsCurrent => InRange(DateTime.Now);

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                format = DateTimeFormats.ShortDateFullYear;

            return $"{StartDate.Format(format)} - {EndDate.Format(format)}";
        }

        public DateTimeRange ConvertToUTC()
        {
            return new DateTimeRange(StartDate.ToUniversalTime(), EndDate.ToUniversalTime());
        }

        public virtual bool InRange(DateTime date, bool includeTime = false)
        {
            if (date == DateTime.MaxValue || date == DateTime.MinValue)
                return false;

            if (!includeTime)
                date = date.Date;

            return (includeTime ? StartDate : StartDate.Date) <= date 
                && (EndDate == DateTime.MaxValue || (includeTime ? EndDate : EndDate.Date) >= date);
        }

        public virtual bool InRange(DateTimeRange dateRange)
        {
            if (dateRange == null)
                return false;

            // started before end date
            if (StartDate <= dateRange.EndDate && (EndDate == DateTime.MaxValue || EndDate >= dateRange.EndDate))
                return true;

            // existed inside date range
            if (StartDate >= dateRange.StartDate && EndDate != DateTime.MaxValue && EndDate <= dateRange.EndDate)
                return true;

            // started before start date
            if (StartDate <= dateRange.StartDate && (EndDate == DateTime.MaxValue || EndDate > dateRange.StartDate))
                return true;

            return false;
        }
    }
}
