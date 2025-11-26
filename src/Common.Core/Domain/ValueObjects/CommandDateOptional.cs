using System;

namespace Common.Core.Domain
{
    public class CommandDateOptional
    {
        public static readonly CommandDateOptional Empty = new CommandDateOptional();

        protected CommandDateOptional() { }

        public CommandDateOptional(int userId) 
            : this(userId, DateTime.UtcNow)
        {
        }

        public CommandDateOptional(int userId, DateTime date)
        {
            UserId = userId;
            Date = date;
        }

        public int? UserId { get; private set; }
        public DateTime? Date { get; private set; }
    }
}
