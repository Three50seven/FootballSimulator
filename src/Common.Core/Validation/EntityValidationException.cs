namespace Common.Core.Validation
{
    [Serializable]
    public class EntityValidationException : ValidationException
    {
        public EntityValidationException(IEnumerable<EntityDataStoreValidationError> validationErrors)
            : this(validationErrors, null)
        {
        }

        public EntityValidationException(IEnumerable<EntityDataStoreValidationError> validationErrors, Exception? innerException)
            : base(new BrokenRulesList(validationErrors), innerException ?? new Exception("Validation failed"))
        {
        }
    }
}
