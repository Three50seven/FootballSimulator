using Common.Core.Validation;

namespace Common.Core.Domain
{
    public class CommandResult : ValueObject<CommandResult>
    {
        protected CommandResult() { }

        public CommandResult(bool succeeded) : this(succeeded, null)
        {

        }

        public CommandResult(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message.SetEmptyToNull() ?? GetDefaultMessage(succeeded);
        }

        public CommandResult(ValidationRule rule) : this(new BrokenRulesList(rule))
        {

        }

        public CommandResult(BrokenRulesList brokenRules)
        {
            BrokenRules = brokenRules ?? new BrokenRulesList();
            Succeeded = BrokenRules.Count == 0;
            Message = Succeeded ? "Success" : string.IsNullOrWhiteSpace(BrokenRules.Message) ? "Failed" : BrokenRules.Message;
        }

        private static string GetDefaultMessage(bool succeeded) => succeeded ? "Success" : "Failed";

        public bool Succeeded { get; protected set; }
        public string Message { get; protected set; }

        public BrokenRulesList BrokenRules { get; protected set; } = new BrokenRulesList();

        public static CommandResult Success() => new CommandResult(true);
        public static CommandResult Success(string message) => new CommandResult(true, message);
        public static CommandResult Fail(BrokenRulesList brokenRules) => new CommandResult(brokenRules);
        public static CommandResult Fail(ValidationRule rule) => new CommandResult(new BrokenRulesList(rule));
        public static CommandResult Fail(string message) => new CommandResult(new ValidationRule(message));
    }
}
