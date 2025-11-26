namespace Common.Core.Domain
{
    public interface IEntityHistoryUpdated
    {
        void OnHistoryUpdate(EntityHistory entityHistory);
    }
}
