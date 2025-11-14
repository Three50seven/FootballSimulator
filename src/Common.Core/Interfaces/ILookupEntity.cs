namespace Common.Core.Domain
{
    public interface ILookupEntity : IEntity<int>, IEntity
    {
        string Name { get; }

        SelectItem ToSelectItem();
    }
}
