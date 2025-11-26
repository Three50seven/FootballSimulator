using Common.Core.Validation;

namespace Common.Core.Domain
{
    public class DataCommandResult<T> : CommandResult where T : class
    {
        public DataCommandResult(bool succeeded) : this(succeeded, default(T))
        {

        }

        public DataCommandResult(bool succeeded, T item) : base(succeeded)
        {
            Item = item;
        }

        public DataCommandResult(bool succeeded, T item, string message) : base(succeeded, message)
        {
            Item = item;
        }

        public DataCommandResult(BrokenRulesList brokenRules) : this(brokenRules, null)
        {

        }

        public DataCommandResult(BrokenRulesList brokenRules, T item) : base(brokenRules)
        {
            Item = item;
        }

        public T Item { get; protected set; }
        public virtual bool ItemExists => Item != null;

        public static DataCommandResult<T> Success(T item) => new DataCommandResult<T>(true, item);
        public static new DataCommandResult<T> Fail(string message) => new DataCommandResult<T>(new BrokenRulesList(new ValidationRule(message)));
        public static new DataCommandResult<T> Fail(ValidationRule rule) => new DataCommandResult<T>(new BrokenRulesList(rule));
        public static new DataCommandResult<T> Fail(BrokenRulesList brokenRules) => new DataCommandResult<T>(brokenRules);
    }
}
