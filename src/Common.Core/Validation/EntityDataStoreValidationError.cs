namespace Common.Core.Validation
{
    public class EntityDataStoreValidationError : PropertyValidationRule
    {
        public string EntityType { get; private set; }

        public string ErrorMessage { get; private set; }

        public override string Message => base.Name + " is invalid. " + ErrorMessage;

        public EntityDataStoreValidationError(string entityType, string propertyName, string errorMessage)
            : base(propertyName, errorMessage)
        {
            EntityType = entityType;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return $"Entity: {EntityType}. Invalid Property: {base.Name}. Message: {ErrorMessage}";
        }
    }
}
