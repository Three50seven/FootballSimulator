namespace Common.Core.Domain
{
    public class UserCommandEvent : ValueObject<UserCommandEvent>
    {
        public DateTime Date { get; private set; }

        public int UserId { get; private set; }

        public IUser? User { get; private set; } // Made nullable to satisfy CS8618

        public static UserCommandEvent Empty => new UserCommandEvent();

        protected UserCommandEvent()
        {
        }

        public UserCommandEvent(int userId)
            : this(userId, DateTime.UtcNow)
        {
        }

        public UserCommandEvent(int userId, DateTime date)
        {
            UserId = userId;
            Date = date;
        }

        public UserCommandEvent(IUser user)
            : this(user, DateTime.UtcNow)
        {
        }

        public UserCommandEvent(IUser user, DateTime date)
        {
            User = user ?? throw new ArgumentNullException("user");
            UserId = user.Id;
            Date = date;
        }

        public UserCommandEvent(CommandDate commandDate)
            : this(commandDate.UserId, commandDate.Date)
        {
        }

        public UserCommandEvent(UserCommandEvent commandEvent)
            : this(commandEvent.User ?? throw new ArgumentNullException(nameof(commandEvent.User)), commandEvent.Date)
        {
        }
    }
}
