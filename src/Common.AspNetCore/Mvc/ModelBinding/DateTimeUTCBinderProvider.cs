using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Common.Core.Validation;
using System;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Custom model binder provider that will return custom binder <see cref="DateTimeUTCModelBinder"/>
    /// for <see cref="DateTime"/> and <see cref="Nullable{DateTime}"/> model properties during binding.
    /// </summary>
    public class DateTimeUTCBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            Guard.IsNotNull(context, nameof(context));

            if (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
                return new BinderTypeModelBinder(typeof(DateTimeUTCModelBinder));

            return null;
        }
    }
}
