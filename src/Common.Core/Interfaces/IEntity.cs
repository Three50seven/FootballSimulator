namespace Common.Core.Domain
{
    public interface IEntity<TId> : IEntity
    {
        TId Id { get; }

        bool IsNew { get; }
    }

    public interface IEntity
    {
    }
}
