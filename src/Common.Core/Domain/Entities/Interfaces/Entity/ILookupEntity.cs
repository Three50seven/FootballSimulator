namespace Common.Core.Domain
{
    /// <summary>
    /// Standard abstraction for simple "lookup" entities that have and Id and Name.
    /// </summary>
    public interface ILookupEntity : IEntity<int>
    {
        string Name { get; }
        SelectItem ToSelectItem();
    }
}
