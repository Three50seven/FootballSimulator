namespace Common.Core.Domain
{
    public class EntityChange : ValueObject<EntityChange>
    {
        public string Description { get; private set; } = string.Empty;

        public string OldValue { get; private set; } = string.Empty;

        public string NewValue { get; private set; } = string.Empty;

        public static EntityChange Empty => new EntityChange();

        protected EntityChange()
        {
        }

        public EntityChange(string description, string oldValue, string newValue)
        {
            Description = description?.SetEmptyToNull() ?? string.Empty;
            OldValue = oldValue?.SetEmptyToNull() ?? string.Empty;
            NewValue = newValue?.SetEmptyToNull() ?? string.Empty;
        }
    }
}
