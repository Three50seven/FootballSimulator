using Common.Core.Validation;

namespace Common.Core.Domain
{
    public class EntityHistory : Entity<long>
    {
        public Guid EntityGuid { get; private set; }

        public int TypeId { get; private set; }

        public EntityType Type { get; private set; } = default!;

        public int CommandTypeId { get; private set; }

        public CommandTypeOption CommandType => (CommandTypeOption)CommandTypeId;

        public UserCommandEvent Event { get; private set; } = default!;

        public List<EntityHistoryChange> Changes { get; private set; } = new List<EntityHistoryChange>();


        protected EntityHistory()
        {
        }

        public EntityHistory(int entityTypeId, CommandTypeOption commandType, Guid entityGuid, UserCommandEvent @event)
        {
            TypeId = entityTypeId;
            CommandTypeId = (int)commandType;
            EntityGuid = entityGuid;
            Event = @event ?? throw new ArgumentNullException("event");
        }

        public void AddChange(EntityProperty value, EntityChange change)
        {
            Guard.IsNotNull(value, "value");
            Guard.IsNotNull(change, "change");
            Changes.Add(new EntityHistoryChange(this, value, change));
        }
    }
}
