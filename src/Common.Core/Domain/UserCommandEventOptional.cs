namespace Common.Core.Domain
{
    public class UserCommandEventOptional : ValueObject<UserCommandEventOptional>
    {
        public DateTime? Date { get; private set; }

        public int? UserId { get; private set; }

        public IUser? User { get; private set; }

        public static UserCommandEventOptional Empty => new UserCommandEventOptional();

        protected UserCommandEventOptional()
        {
            Date = default;
            UserId = default;
        }

        public UserCommandEventOptional(int? userId)
            : this(userId, DateTime.UtcNow)
        {
        }

        public UserCommandEventOptional(int? userId, DateTime? date)
        {
            UserId = userId.CleanForNull();
            Date = date;
        }

        public UserCommandEventOptional(IUser user)
            : this(user, DateTime.UtcNow)
        {
        }

        public UserCommandEventOptional(IUser user, DateTime? date)
        {
            User = user ?? throw new ArgumentNullException("user");
            UserId = user.Id;
            Date = date;
        }

        public UserCommandEventOptional(CommandDate commandDate)
            : this(commandDate.UserId, commandDate.Date)
        {
        }

        public UserCommandEventOptional(CommandDateOptional commandDate)
            : this(commandDate.UserId, commandDate.Date)
        {
        }

        public UserCommandEventOptional(UserCommandEvent commandEvent)
            : this(commandEvent.User ?? throw new ArgumentNullException(nameof(commandEvent.User)), commandEvent.Date)
        {
        }

        public UserCommandEventOptional(UserCommandEventOptional commandEvent)
            : this(commandEvent.User ?? throw new ArgumentNullException(nameof(commandEvent.User)), commandEvent.Date)
        {
        }
    }
}
