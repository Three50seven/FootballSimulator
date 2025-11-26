using Common.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace Common.Core.Annotations
{
    /// <summary>
    /// Provides conditional validation based on related property value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredIfAttribute : RequiredAttribute
    {
        public RequiredIfAttribute(string dependantProperty, object dependantPropertyValue)
        {
            DependantProperty = dependantProperty;
            DependantPropertyValue = dependantPropertyValue;
            ErrorMessage = "{0} is required.";
        }

        public string DependantProperty { get; private set; }

        public object DependantPropertyValue { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the attribute requires validation context.
        /// </summary>
        /// <returns><c>true</c> if the attribute requires validation context; otherwise, <c>false</c>.</returns>
        public override bool RequiresValidationContext
        {
            get { return true; }
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>
        /// An instance of the formatted error message.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                base.ErrorMessageString,
                name,
                DependantProperty,
                DependantPropertyValue);
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            Guard.IsNotNull(validationContext, nameof(validationContext));

            PropertyInfo dependantPropertyInfo = validationContext.ObjectType.GetProperty(DependantProperty)!;
            if (dependantPropertyInfo == null)
                return new System.ComponentModel.DataAnnotations.ValidationResult($"Internal Validation Error - Could not find a property named '{DependantProperty}' to validate a conditional required field.");

            object dependantValue = dependantPropertyInfo.GetValue(validationContext.ObjectInstance)!;

            // check if this value is actually required and validate it
            if (object.Equals(dependantValue, DependantPropertyValue) && !ValueIsSet(value!))
                return new System.ComponentModel.DataAnnotations.ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return System.ComponentModel.DataAnnotations.ValidationResult.Success!;
        }

        protected virtual bool ValueIsSet(object value)
        {
            return !(value == null || (value is string && string.IsNullOrWhiteSpace(value as string)));
        }
    }
}
