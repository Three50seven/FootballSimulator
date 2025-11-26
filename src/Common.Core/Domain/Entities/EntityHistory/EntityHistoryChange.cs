namespace Common.Core.Domain
{
    public class EntityHistoryChange : Entity<long>
    {
        protected EntityHistoryChange () 
        { 
            EntityHistory = null!;
        }

        public EntityHistoryChange(
            EntityHistory entityHistory,
            EntityProperty property,
            EntityChange change)
        {
            EntityHistory = entityHistory;
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Change = change ?? throw new ArgumentNullException(nameof(change));
        }

        public long EntityHistoryId { get; private set; }
        public EntityHistory EntityHistory { get; private set; }

        public EntityProperty Property { get; private set; } = EntityProperty.Empty;
        public EntityChange Change { get; private set; } = EntityChange.Empty;
    }
}
