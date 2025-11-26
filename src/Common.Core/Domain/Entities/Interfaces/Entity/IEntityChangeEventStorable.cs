namespace Common.Core.Domain
{
    public interface IEntityChangeEventStorable
    {
        ChangeEvents ChangeEvents { get; }
    }
}
