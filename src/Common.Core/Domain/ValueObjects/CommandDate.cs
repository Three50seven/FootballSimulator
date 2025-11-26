using System;

namespace Common.Core.Domain
{
    public class CommandDate : CommandDate<int>
    {
        /// <summary>
        /// 1 used as default for system user id
        /// </summary>
        public static CommandDate SystemEvent => new CommandDate(new DefaultUser().Id);
        public static CommandDate Empty => new CommandDate();

        protected CommandDate() { }

        public CommandDate(int userId) : this(userId, DateTime.UtcNow)
        {
        }

        public CommandDate(int userId, DateTime date) : base(userId, date)
        {
        }

        public new CommandDate Copy()
        {
            return new CommandDate(UserId, Date);
        }
    }

    public class CommandDate<T> : ValueObject<CommandDate<T>>
    {
        protected CommandDate() { }

        public CommandDate(T userId) : this(userId, DateTime.UtcNow)
        {

        }

        public CommandDate(T userId, DateTime date)
        {
            UserId = userId;
            Date = date;
        }

        public T UserId { get; private set; }
        public DateTime Date { get; private set; }

        public CommandDate<T> Copy()
        {
            return new CommandDate<T>(UserId, Date);
        }
    }
}
