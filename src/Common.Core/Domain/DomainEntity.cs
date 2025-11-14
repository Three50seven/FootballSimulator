namespace Common.Core.Domain
{
    public abstract class DomainEntity : DomainEntity<int>, IDomainEntity, IDomainEntity<int>, IEntity<int>, IEntity, IEntityGuid
    {
        protected DomainEntity() { }
        protected DomainEntity(int id) : base(id) { }
        protected DomainEntity(Guid guid) : base(guid) { }
    }

    public abstract class DomainEntity<TKey> : Entity<TKey>, IDomainEntity<TKey>, IEntity<TKey>, IEntity, IEntityGuid, IHistoricalEntity, IEntityConcurrency, IEntityHistoryUpdated
    {
        public Guid Guid { get; private set; }

        public override bool IsNew => object.Equals(base.Id, default(TKey));

        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        protected DomainEntity()
            : this(Guid.NewGuid())
        {
        }

        protected DomainEntity(TKey id)
            : base(id)
        {
            Guid = Guid.NewGuid();
            RowVersion = Array.Empty<byte>();
        }

        protected DomainEntity(Guid guid)
        {
            Guid = guid;
            RowVersion = Array.Empty<byte>();
        }

        public virtual void OnHistoryUpdate(EntityHistory entityHistory)
        {
        }
    }
}
