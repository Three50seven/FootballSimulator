namespace Common.Core.Domain
{
    public class CommandDate : CommandDate<int>
    {
        //
        // Summary:
        //     1 used as default for system user id
        public static CommandDate SystemEvent => new CommandDate(new DefaultUser().Id);

        public static CommandDate Empty => new CommandDate();

        protected CommandDate()
        {
        }

        public CommandDate(int userId)
            : this(userId, DateTime.UtcNow)
        {
        }

        public CommandDate(int userId, DateTime date)
            : base(userId, date)
        {
        }

        public new CommandDate Copy()
        {
            return new CommandDate(base.UserId, base.Date);
        }
    }

    public class CommandDate<T> : ValueObject<CommandDate<T>>
    {
        public T UserId { get; private set; } = default!;
        public DateTime Date { get; private set; }

        protected CommandDate()
        {
        }

        public CommandDate(T userId)
            : this(userId, DateTime.UtcNow)
        {
        }

        public CommandDate(T userId, DateTime date)
        {
            UserId = userId;
            Date = date;
        }

        public CommandDate<T> Copy()
        {
            return new CommandDate<T>(UserId, Date);
        }
    }
}
