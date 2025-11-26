using Common.Core.Validation;

namespace Common.Core.Domain
{
    public class EntityHistory : Entity<long>
    {
        protected EntityHistory() 
        {
            TypeId = 0;
            CommandTypeId = 0;
            EntityGuid = Guid.Empty;
            Event = null!;
        }

        public EntityHistory(
            int entityTypeId, 
            CommandTypeOption commandType, 
            Guid entityGuid,
            UserCommandEvent @event)
        {
            TypeId = entityTypeId;
            CommandTypeId = (int)commandType;
            EntityGuid = entityGuid;
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
        }

        public Guid EntityGuid { get; private set; }

        public int TypeId { get; private set; }
        public EntityType? Type { get; private set; }
        
        public int CommandTypeId { get; private set; }
        public CommandTypeOption CommandType => (CommandTypeOption)CommandTypeId;
        public UserCommandEvent Event { get; private set; }

        public IEnumerable<EntityHistoryChange> Changes { get; private set; } = new List<EntityHistoryChange>();

        public void AddChange(EntityProperty value, EntityChange change)
        {
            Guard.IsNotNull(value, nameof(value));
            Guard.IsNotNull(change, nameof(change));

            if (Changes is List<EntityHistoryChange> list)
            {
                list.Add(new EntityHistoryChange(this, value, change));
            }
        }
    }
}
