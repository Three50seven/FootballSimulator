using Common.Core.Validation;
using System;
using System.Collections.Generic;

namespace Common.Core
{
    [Serializable]
    public class EntityValidationException : ValidationException
    {
        public EntityValidationException(IEnumerable<EntityDataStoreValidationError> validationErrors) 
            : this(validationErrors, null)
        {

        }

        public EntityValidationException(IEnumerable<EntityDataStoreValidationError> validationErrors, Exception innerException) 
            : base(new BrokenRulesList(validationErrors), innerException)
        {

        }
    }
}
