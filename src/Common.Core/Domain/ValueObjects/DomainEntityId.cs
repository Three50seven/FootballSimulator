using System;

namespace Common.Core.Domain
{
    public class DomainEntityId : ValueObject<DomainEntityId>
    {
        protected DomainEntityId() { }

        public DomainEntityId(int id, Guid guid)
        {
            Id = id;
            Guid = guid;
        }

        public int Id { get; private set; }
        public Guid Guid { get; private set; }

        public override string ToString()
        {
            return $"Id: {Id} Guid: {Guid}";
        }
    }
}
