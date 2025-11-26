using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using Common.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Optional base class for creating an implementation for <see cref="IValidationAttributeAdapterFactory"/>.
    /// Validates the type of attribute supplied against the generic type <typeparamref name="TAttr"/>.
    /// </summary>
    /// <typeparam name="TAttr"></typeparam>
    public abstract class ValidationAttributeAdapterFactoryBase<TAttr> : IValidationAttributeAdapterFactory
        where TAttr : ValidationAttribute
    {
        public virtual IAttributeAdapter Create(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            Guard.IsNotNull(attribute, nameof(attribute));

            if (attribute.GetType() != typeof(TAttr))
            {
                throw new InvalidOperationException($@"Attribute type mismatch. Supplied attribute for creating adapter 
using {GetType().FullName} must be of type {typeof(TAttr).FullName} but was of type {attribute.GetType().FullName}.");
            }

            return CreateAdapter(attribute as TAttr, stringLocalizer);
        }

        protected abstract IAttributeAdapter CreateAdapter(TAttr attribute, IStringLocalizer stringLocalizer);
    }
}
