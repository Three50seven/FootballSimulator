namespace Common.Core.Domain
{
    public interface IDomainEntity : IDomainEntity<int>, IEntity<int>, IEntity, IEntityGuid
    {
    }
    public interface IDomainEntity<TKey> : IEntity<TKey>, IEntity, IEntityGuid
    {
    }
}
