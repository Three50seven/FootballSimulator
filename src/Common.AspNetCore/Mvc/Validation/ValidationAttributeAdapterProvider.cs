using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Custom validation adapter provider so custom attributes and associated custom providers are used.
    /// Ref - https://stackoverflow.com/questions/39097786/dataannotationsmodelvalidatorprovider-registeradapter-in-asp-net-core-mvc
    /// </summary>
    public class ValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider;
        private readonly IReadOnlyDictionary<Type, IValidationAttributeAdapterFactory> _adapterLookup;

        public ValidationAttributeAdapterProvider(
            IValidationAttributeAdapterProvider baseProvider, 
            IReadOnlyDictionary<Type, IValidationAttributeAdapterFactory> adapterLookup)
        {
            _baseProvider = baseProvider;
            _adapterLookup = adapterLookup;
        }

        public virtual IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (_adapterLookup.TryGetValue(attribute.GetType(), out IValidationAttributeAdapterFactory adapterFactory))
                return adapterFactory.Create(attribute, stringLocalizer);

            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
