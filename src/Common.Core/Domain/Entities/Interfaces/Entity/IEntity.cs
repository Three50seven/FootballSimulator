namespace Common.Core.Domain
{
    public interface IEntity
    {

    }

    public interface IEntity<TId> : IEntity
    {
        TId Id { get; }
        bool IsNew { get; }
    }
}
