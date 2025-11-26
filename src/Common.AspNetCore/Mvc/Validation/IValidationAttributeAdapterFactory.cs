using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Factory to build custom <see cref="IAttributeAdapter"/> per provided validation attribute.
    /// This allows for custom adapters around custom attributes.
    /// The adapters can be used to provide client-side properties and values around a custom validation attribute.
    /// Utilized in custom adapter class <see cref="ValidationAttributeAdapterProvider"/> around the custom provider.
    /// </summary>
    public interface IValidationAttributeAdapterFactory
    {
        /// <summary>
        /// Build custom adapter based on the validation attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="stringLocalizer"></param>
        /// <returns></returns>
        IAttributeAdapter Create(ValidationAttribute attribute, IStringLocalizer stringLocalizer);
    }
}
