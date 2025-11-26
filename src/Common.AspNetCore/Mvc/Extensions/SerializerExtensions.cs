using Microsoft.AspNetCore.Mvc.ModelBinding;
using Common.Core;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore.Mvc
{
    public static class SerializerExtensions
    {
        /// <summary>
        /// Serialize ModelState dictionary. Dictionary is converted to local DTO list.
        /// Call <see cref="DeserializeModelState(ISerializer, string)"/> to deserialize.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string SerializeModelState(this ISerializer serializer, ModelStateDictionary modelState)
        {
            if (serializer == null || modelState == null)
                return null;

            var modelValueList = modelState.Select(kvp => new ModelStateTransferValue
            {
                Key = kvp.Key,
                AttemptedValue = kvp.Value.AttemptedValue,
                RawValue = kvp.Value.RawValue,
                ErrorMessages = kvp.Value.Errors.Select(err => err.ErrorMessage),
            });

            return serializer.Serialize(modelValueList);
        }

        /// <summary>
        /// Deserialize ModelState dictionary. Deserialized to a local DTO list and then applied to a ModelState dictionary.
        /// Called to deserialize ModelState after calling <see cref="SerializeModelState(ISerializer, ModelStateDictionary)"/>.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="serializedModelState"></param>
        /// <returns></returns>
        public static ModelStateDictionary DeserializeModelState(this ISerializer serializer, string serializedModelState)
        {
            if (serializer == null || string.IsNullOrWhiteSpace(serializedModelState))
                return null;

            var modelValueList = serializer.Deserialize<IEnumerable<ModelStateTransferValue>>(serializedModelState);
            var modelState = new ModelStateDictionary();

            foreach (var item in modelValueList)
            {
                modelState.SetModelValue(item.Key, item.RawValue, item.AttemptedValue);
                foreach (var error in item.ErrorMessages)
                {
                    modelState.AddModelError(item.Key, error);
                }
            }

            return modelState;
        }
    }
}