namespace Common.Core.Domain
{
    public class EntityHistoryChange : Entity<long>
    {
        public long EntityHistoryId { get; private set; }

        public EntityHistory EntityHistory { get; private set; } = null!;

        public EntityProperty Property { get; private set; } = EntityProperty.Empty;

        public EntityChange Change { get; private set; } = EntityChange.Empty;

        protected EntityHistoryChange()
        {
        }

        public EntityHistoryChange(EntityHistory entityHistory, EntityProperty property, EntityChange change)
        {
            EntityHistory = entityHistory;
            Property = property ?? throw new ArgumentNullException("property");
            Change = change ?? throw new ArgumentNullException("change");
        }
    }
}
