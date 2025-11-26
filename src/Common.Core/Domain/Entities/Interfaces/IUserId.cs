namespace Common.Core.Domain
{
    public interface IUserId : IUserId<int>
    {
    }

    public interface IUserId<TKey>
    {
        TKey Id { get; }
    }
}
