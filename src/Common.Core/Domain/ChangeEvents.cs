using Common.Core.Validation;

namespace Common.Core.Domain
{
    public class ChangeEvents : ValueObject<ChangeEvents>
    {
        public UserCommandEvent Created { get; private set; }

        public UserCommandEvent Updated { get; protected set; } = UserCommandEvent.Empty;

        public static ChangeEvents Empty => new ChangeEvents(UserCommandEvent.Empty, UserCommandEvent.Empty);

        protected ChangeEvents()
        {
            Created = UserCommandEvent.Empty;
            Updated = UserCommandEvent.Empty;
        }

        public ChangeEvents(UserCommandEvent created, UserCommandEvent updated)
        {
            Created = created ?? UserCommandEvent.Empty;
            Updated = updated ?? UserCommandEvent.Empty;
        }

        public ChangeEvents CreateFromHistory(EntityHistory entityHistory)
        {
            Guard.IsNotNull(entityHistory, "entityHistory");
            switch (entityHistory.CommandType)
            {
                case CommandTypeOption.Added:
                    return new ChangeEvents(new UserCommandEvent(entityHistory.Event.UserId, entityHistory.Event.Date), new UserCommandEvent(entityHistory.Event.UserId, entityHistory.Event.Date));
                case CommandTypeOption.Updated:
                case CommandTypeOption.Deleted:
                    return new ChangeEvents(new UserCommandEvent(Created.UserId, Created.Date), new UserCommandEvent(entityHistory.Event.UserId, entityHistory.Event.Date));
                default:
                    throw new UnsupportedEnumException(entityHistory.CommandType);
            }
        }
    }
}
