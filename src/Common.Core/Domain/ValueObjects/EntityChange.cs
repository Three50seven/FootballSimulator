namespace Common.Core.Domain
{
    public class EntityChange : ValueObject<EntityChange>
    {
        protected EntityChange() { }

        public EntityChange(string description, string oldValue, string newValue)
        {
            Description = description.SetEmptyToNull();
            OldValue = oldValue.SetEmptyToNull();
            NewValue = newValue.SetEmptyToNull();
        }

        public string Description { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }

        public static EntityChange Empty => new EntityChange();
    }
}
