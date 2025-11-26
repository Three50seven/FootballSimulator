using System;

namespace Common.Core.Domain
{
    public class EntityPropertyChange : ValueObject<EntityPropertyChange>
    {
        protected EntityPropertyChange() { }

        public EntityPropertyChange(
            EntityProperty property,
            EntityChange change)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Change = change ?? throw new ArgumentNullException(nameof(change));
        }

        public EntityProperty Property { get; private set; }
        public EntityChange Change { get; private set; }
    }
}
