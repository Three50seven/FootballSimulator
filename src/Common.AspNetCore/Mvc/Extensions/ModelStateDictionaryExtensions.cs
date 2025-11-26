using Microsoft.AspNetCore.Mvc.ModelBinding;
using Common.Core.Validation;

namespace Common.AspNetCore.Mvc
{
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// Append list of broken validation rules to <paramref name="modelState"/>.
        /// Typically the validation rules will be a <see cref="BrokenRulesList"/>.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="validationRules"></param>
        /// <returns></returns>
        public static ModelStateDictionary AddValidationRuleErrors(this ModelStateDictionary modelState, IEnumerable<ValidationRule> validationRules)
        {
            if (modelState == null)
                return null!;

            foreach (var rule in validationRules)
            {
                AddValidationRuleError(modelState, rule);
            }

            return modelState;
        }

        /// <summary>
        /// Append a broken validation rul to <paramref name="modelState"/>.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static ModelStateDictionary AddValidationRuleError(this ModelStateDictionary modelState, ValidationRule rule)
        {
            if (modelState == null)
                return null!;

            var propertyRule = rule as PropertyValidationRule;
            if (propertyRule! != null!)
                modelState.AddModelError(propertyRule.Name, propertyRule.Message);
            else
                modelState.AddModelError(BrokenRulesList.ModelStateKey, rule.Message);

            return modelState;
        }
    
        /// <summary>
        /// Convert list of errors found in <paramref name="modelState"/> to a new <see cref="BrokenRulesList"/>.
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static BrokenRulesList ToBrokenRulesList(this ModelStateDictionary modelState)
        {
            if (modelState == null || modelState.IsValid)
                return BrokenRulesList.Empty;

            var brokenRules = new BrokenRulesList();
            foreach (var key in modelState.Keys)
            {
                foreach (var error in modelState[key].Errors)
                {
                    brokenRules.AddUnique(new ValidationRule(error.ErrorMessage));
                }
            }

            return brokenRules;
        }

        /// <summary>
        /// Get all error message found on the model state based <see cref="ModelStateDictionary.Keys"/>.
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetAllErrors(this ModelStateDictionary modelState)
        {
            foreach (var key in modelState.Keys)
            {
                foreach (var error in modelState[key].Errors)
                {
                    yield return error.ErrorMessage;
                }
            }
        }

        /// <summary>
        /// Get any error messages on the modelstate from the given key.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetErrorsByKey(this ModelStateDictionary modelState, string key)
        {
            Guard.IsNotNull(key, nameof(key));

            if (modelState.TryGetValue(key, out var errors))
                return errors.Errors.Select(e => e.ErrorMessage);
            else
                return Enumerable.Empty<string>();
        }
    }
}
