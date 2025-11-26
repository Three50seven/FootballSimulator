namespace Common.Core.Domain
{
    /// <summary>
    /// Entity with standard int Id as well as Guid identifier.
    /// Used for important, managable entities on the domain.
    /// </summary>
    public interface IDomainEntity : IDomainEntity<int>
    {
    }

    /// <summary>
    /// Entity with custom Id (typically int or long) as well as Guid identifier.
    /// Used for important, managable entities on the domain.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IDomainEntity<TKey> : IEntity<TKey>, IEntityGuid
    {

    }
}
