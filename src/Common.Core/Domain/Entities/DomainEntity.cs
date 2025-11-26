namespace Common.Core.Domain
{
    /// <summary>
    /// Base implementation for entity with standard int Id as well as Guid identifier.
    /// Used for important, managable entities on the domain.
    /// Includes row version and historical callback.
    /// </summary>
    public abstract class DomainEntity : DomainEntity<int>, IDomainEntity
    {
        protected DomainEntity()
        {
        }

        protected DomainEntity(int id) 
            : base(id)
        {
        }

        protected DomainEntity(Guid guid) 
            : base(guid)
        {
        }
    }

    /// <summary>
    /// Base implementation for entity with standard int Id as well as Guid identifier.
    /// Used for important, managable entities on the domain.
    /// Includes row version and historical callback.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class DomainEntity<TKey> : Entity<TKey>, IDomainEntity<TKey>, IHistoricalEntity
    {
        protected DomainEntity() 
            : this(Guid.NewGuid())
        {
        }

        protected DomainEntity(TKey id) 
            : base(id)
        {
            Guid = Guid.NewGuid();
            RowVersion = null!;
        }

        protected DomainEntity(Guid guid)
        {
            Guid = guid;
            RowVersion = null!;
        }

        public Guid Guid { get; private set; }

        public override bool IsNew => object.Equals(Id, default(TKey));

        public byte[] RowVersion { get; private set; }
        
        public virtual void OnHistoryUpdate(EntityHistory entityHistory) { }
    }
}
