using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DateRangeAttribute : ValidationAttribute
    {
        public DateRangeAttribute(string endDatePropName)
        {
            EndDatePropName = string.IsNullOrWhiteSpace(endDatePropName) ? "EndDate" : endDatePropName.Trim();
        }

        public string EndDatePropName { get; private set; }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success!;

            if (!(value is DateTime))
                return ValidationResult.Success!;

            var startDate = (DateTime)value;
            var endDateProp = validationContext.ObjectType.GetProperty(EndDatePropName);
            if (endDateProp == null)
                throw new MissingMemberException($"End Date property '{EndDatePropName}' not found.");

            var endDate = endDateProp.GetValue(validationContext.ObjectInstance);
            if (endDate == null)
                return ValidationResult.Success!;

            if ((DateTime)endDate < startDate)
            {
                var message = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(message);
            }

            return ValidationResult.Success!;
        }
    }
}