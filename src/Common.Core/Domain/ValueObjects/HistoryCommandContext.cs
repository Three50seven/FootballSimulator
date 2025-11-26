using System;
using System.Collections.Generic;

namespace Common.Core.Domain
{
    public class HistoryCommandContext : ValueObject<HistoryCommandContext>
    {
        protected HistoryCommandContext() { }

        public HistoryCommandContext(
            int entityTypeId,
            IHistoricalEntity entity,
            CommandTypeOption type,
            int userId = default,
            IEnumerable<EntityPropertyChange> changedProps = null,
            bool storeEventRecords = true)
        {
            EntityTypeId = entityTypeId;
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            Type = type;
            UserId = userId;
            ChangedProperties = changedProps ?? new List<EntityPropertyChange>();
            StoreEventRecords = storeEventRecords;
        }

        public int EntityTypeId { get; private set; }
        public IHistoricalEntity Entity { get; private set; }
        public CommandTypeOption Type { get; private set; }
        public int UserId { get; private set; }
        public IEnumerable<EntityPropertyChange> ChangedProperties { get; private set; } = new List<EntityPropertyChange>();
        public bool StoreEventRecords { get; private set; }

        public EntityHistory ToEntityHistory(int currentUserId)
        {
            var entityHistory = new EntityHistory(
                EntityTypeId,
                Type,
                Entity.Guid,
                new UserCommandEvent(UserId == default ? currentUserId : UserId));

            if (ChangedProperties != null)
            {
                foreach (var prop in ChangedProperties)
                {
                    entityHistory.AddChange(prop.Property, prop.Change);
                }
            }
            
            return entityHistory;
        }
    }
}
