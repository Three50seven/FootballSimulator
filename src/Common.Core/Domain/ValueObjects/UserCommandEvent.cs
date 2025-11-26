using System;

namespace Common.Core.Domain
{
    public class UserCommandEvent : ValueObject<UserCommandEvent>
    {
        protected UserCommandEvent() { }

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
            User = user ?? throw new ArgumentNullException(nameof(user));
            UserId = user.Id;
            Date = date;
        }

        public UserCommandEvent(CommandDate commandDate)
            : this(commandDate.UserId, commandDate.Date)
        {

        }

        public UserCommandEvent(UserCommandEvent commandEvent)
            : this(commandEvent.User, commandEvent.Date)
        {

        }

        public DateTime Date { get; private set; }
        public int UserId { get; private set; }
        public IUser User { get; private set; }

        public static UserCommandEvent Empty => new UserCommandEvent();
    }
}
