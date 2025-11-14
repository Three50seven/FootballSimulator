using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public abstract class FSDataEntity : DomainEntity, IEntityChangeEventStorable
    {
        public ChangeEvents ChangeEvents { get; protected set; } = ChangeEvents.Empty;

        public override void OnHistoryUpdate(EntityHistory entityHistory)
        {
            ChangeEvents = ChangeEvents.CreateFromHistory(entityHistory);
        }

        public void UpdateChangeEvents(int userId)
        {
            if (IsNew)
                ChangeEvents = new ChangeEvents(new UserCommandEvent(userId), new UserCommandEvent(userId));
            else
                ChangeEvents = new ChangeEvents(new UserCommandEvent(ChangeEvents.Created.UserId, ChangeEvents.Created.Date), new UserCommandEvent(userId));
        }
    }
}
