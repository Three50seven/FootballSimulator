using System.ComponentModel.DataAnnotations;

namespace Common.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GreaterThanZeroAttribute : ValidationAttribute
    {
        public string RequiredIfPropertyName = null!;

        public GreaterThanZeroAttribute() : base("Value must be greater than 0.") { }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success!;

            var message = FormatErrorMessage(validationContext.DisplayName);

            //if Required is conditioned based on a property, look it up to determine if required
            if (!string.IsNullOrEmpty(RequiredIfPropertyName))
            {
                Object instance = validationContext.ObjectInstance;

                var propertyInfo = instance?.GetType().GetProperty(RequiredIfPropertyName)
                    ?? throw new InvalidOperationException($"Property '{RequiredIfPropertyName}' not found.");
                object? propertyvalue = propertyInfo.GetValue(instance, null);

                //if the value of the property is false (ex: checkbox was not checked) validation passes
                if (propertyvalue?.ToString() == bool.FalseString)
                {
                    return ValidationResult.Success!;
                }
            }

            if (value is int)
            {
                var integer = Convert.ToInt32(value);
                if (integer <= 0)
                    return new ValidationResult(message);
            }
            else if (value is decimal)
            {
                var dec = Convert.ToDecimal(value);
                if (dec <= 0)
                    return new ValidationResult(message);
            }
            else if (value is double)
            {
                var dbl = Convert.ToDouble(value);
                if (dbl <= 0)
                    return new ValidationResult(message);
            }

            return ValidationResult.Success!;
        }
    }
}
