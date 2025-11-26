using Microsoft.AspNetCore.Mvc.ModelBinding;
using Common.Core.Validation;

namespace Common.AspNetCore.Mvc
{
    public static class ModelBindingContextExtensions
    {
        public static ValueProviderResult GetValueResult(this ModelBindingContext bindingContext)
        {
            Guard.IsNotNull(bindingContext, nameof(bindingContext));
            return bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        }
    }
}
