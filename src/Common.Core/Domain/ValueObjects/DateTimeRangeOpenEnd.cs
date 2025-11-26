using Common.Core.Validation;
using System;

namespace Common.Core.Domain
{
    public class DateTimeRangeOpenEnd : ValueObject<DateTimeRangeOpenEnd>
    {
        protected DateTimeRangeOpenEnd() { }

        public DateTimeRangeOpenEnd(DateTime startDate) : this(startDate, null)
        { }

        public DateTimeRangeOpenEnd(DateTime startDate, DateTime? endDate)
        {
            if (endDate != null)
                Guard.IsValidDateRange(startDate, (DateTime)endDate);

            StartDate = startDate;
            EndDate = endDate.HasValue() ? endDate : null;
        }

        public DateTimeRangeOpenEnd(DateTime startDate, TimeSpan span)
            : this(startDate, startDate.Add(span))
        {
        }

        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        public bool IsCurrent => InRange(DateTime.Now);

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                format = DateTimeFormats.ShortDateFullYear;

            if (EndDate.HasValue())
                return $"{StartDate.Format(format)} - {EndDate.Format(format)}";
            else
                return StartDate.Format(format);
        }

        public void ConvertToUTC()
        {
            StartDate = StartDate.ToUniversalTime();
            if (EndDate != null)
                EndDate = ((DateTime)EndDate).ToUniversalTime();
        }

        public virtual bool InRange(DateTime date, bool includeTime = false)
        {
            if (date == DateTime.MaxValue || date == DateTime.MinValue)
                return false;

            if (!includeTime)
                date = date.Date;

            return (includeTime ? StartDate : StartDate.Date) <= date 
                && (EndDate == null || (includeTime ? EndDate.Value : EndDate.Value.Date) >= date);
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
