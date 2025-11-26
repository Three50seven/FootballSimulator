namespace Common.Core.Domain
{
    public interface IHistoricalEntity : IEntityGuid, IEntityConcurrency, IEntityHistoryUpdated
    {
    }
}
