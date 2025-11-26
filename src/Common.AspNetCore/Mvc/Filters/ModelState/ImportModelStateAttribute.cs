using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.AspNetCore.Mvc
{
    /// <summary>
    /// Checks for a serialized ModelState from TempData on OnActionExecuted event.
    /// If serialized ModelState is present and result is ViewResult, the serialized ModelState is merged with the resulting ModelState.
    /// Setting <see cref="TrySetModelValues"/> will attempt to update the model object values. 
    /// If model inherits <see cref="IKeyValuesUpdatable"/>, it will be called to update itself, otherwise reflection is used.
    /// Attribute is intended to be used on form GET actions where a POST action redirects and is decorated with <see cref="ExportInvalidModelStateAttribute"/>.
    /// </summary>
    public class ImportModelStateAttribute : ModelStateTransferringAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trySetModelValues">Whether or not the values from ModelState should be applied to the model itself. Defaults to false.</param>
        public ImportModelStateAttribute(bool trySetModelValues = false)
        {
            TrySetModelValues = trySetModelValues;
        }

        /// <summary>
        /// Whether or not the values from ModelState should be applied to the model itself. Defaults to false.
        /// </summary>
        public bool TrySetModelValues { get; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as Controller;

            if (controller?.TempData[Key] is string serializedModelState)
            {
                if (filterContext.Result is ViewResult)
                {
                    var serializer = (ISerializer)filterContext.HttpContext.RequestServices.GetService(typeof(ISerializer)) ?? throw new InvalidOperationException($"Required service implementation not found for {typeof(ISerializer).FullName}.");
                    var modelState = serializer.DeserializeModelState(serializedModelState);
                    filterContext.ModelState.Merge(modelState);

                    if (TrySetModelValues)
                    {
                        var model = (filterContext.Result as ViewResult).Model;
                        if (model != null)
                        {
                            // allow for customization using an interface to update the model from the modelstate key values
                            if (model is IKeyValuesUpdatable keyValueUpdatable)
                            {
                                keyValueUpdatable.UpdateValues(filterContext.ModelState.Select(ms =>
                                    new KeyValuePair<string, string>(ms.Key, ms.Value.AttemptedValue)));
                            }
                            else
                            {
                                // otherwise, use reflection and attempt to update all simpletype properties on the model
                                var valueParser = (IValueParser)filterContext.HttpContext.RequestServices.GetService(typeof(IValueParser)) ?? throw new InvalidOperationException($"Required service implementation not found for {typeof(IValueParser).FullName}.");
                                var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                                .Where(p => p.PropertyType.IsSimpleType());

                                foreach (var property in properties)
                                {
                                    if (filterContext.ModelState.TryGetValue(property.Name, out ModelStateEntry entry)
                                        && valueParser.TryParse(entry.AttemptedValue, property.PropertyType, out object value))
                                    {
                                        property.SetValue(model, value);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    controller?.TempData?.Remove(Key);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
