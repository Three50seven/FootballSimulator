using Microsoft.AspNetCore.Mvc.ModelBinding;
using Common.Core;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Custom model binder that ensure the supplied <see cref="DateTime"/> or <see cref="Nullable{DateTime}"/>
    /// are adjusted to UTC time.
    /// </summary>
    public class DateTimeUTCModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.GetValueResult();
            if (valueResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

            DateTime date;

            // if not formatting, parse normally, else parse while adjusting to UTC time
            if (ShouldIgnoreFormatting(bindingContext.ModelMetadata))
            {
                if (!DateTime.TryParse(valueResult.FirstValue, out date))
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    return Task.CompletedTask;
                }
            }
            else if (!DateTime.TryParse(valueResult.FirstValue, null, DateTimeStyles.AdjustToUniversal, out date))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
               
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, date, valueResult.FirstValue);
            bindingContext.Result = ModelBindingResult.Success(date);

            return Task.CompletedTask;
        }

        private static bool ShouldIgnoreFormatting(ModelMetadata metaData)
        {
            return metaData.HasCustomAttribute<IgnoreDateTimeFormattingAttribute>();
        }
    }
}
