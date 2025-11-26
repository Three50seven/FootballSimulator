using Common.Core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Common.Core
{
    public class DocumentRequiredAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var message = FormatErrorMessage(ErrorMessage!);

            if (value == null)
                return new ValidationResult(message);

            if (!(value is DocumentInputItem))
                return new ValidationResult("DocumentRequired can only be applied to values of type DocumentInputItem.");

            var docValue = (value as DocumentInputItem);
            if (!docValue!.IsSet)
                return new ValidationResult(message);

            return ValidationResult.Success!;
        }
    }
}
