using System;

namespace Common.Core.Domain
{
    public class Age : ValueObject<Age>
    {
        protected Age() { }

        public Age(DateTime dateOfBirth) : this(dateOfBirth, DateTime.Now)
        {

        }

        public Age(DateTime dateOfBirth, DateTime date)
        {
            if (!dateOfBirth.HasValue())
                dateOfBirth = date;

            DateOfBirth = dateOfBirth;

            int months = date.Month - dateOfBirth.Month;
            int years = date.Year - dateOfBirth.Year;

            if (date.Day < dateOfBirth.Day)
                months--;

            if (months < 0)
            {
                years--;
                months += 12;
            }

            Years = years < 0 ? 0 : years;
            Months = months < 0 ? 0 : months;
        }

        public Age(int years, int months)
        {
            Years = years < 0 ? 0 : years;
            Months = months < 0 ? 0 : months;
        }

        public int Years { get; private set; }
        public int Months { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public virtual int TotalMonths => (Years * 12) + Months;

        public static Age Empty => new Age();

        public override string ToString()
        {
            return string.Concat(Years, " year", Years > 1 ? "s" : "", Months > 0 ? string.Concat(", ", Months, " month", Months > 1 ? "s" : "") : "");
        }
    }
}
