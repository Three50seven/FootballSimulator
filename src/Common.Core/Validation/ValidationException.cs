namespace Common.Core.Validation
{
    [Serializable]
    public class ValidationException : Exception
    {
        public BrokenRulesList BrokenRules { get; private set; }

        public virtual string FriendlyMessage
        {
            get
            {
                string? text = BrokenRules?.Message;
                return string.IsNullOrWhiteSpace(text) ? base.Message : text;
            }
        }

        public ValidationException(string message)
            : this(new ValidationRule(message))
        {
        }

        public ValidationException(ValidationRule brokenRule)
            : this(new BrokenRulesList(new List<ValidationRule> { brokenRule }), null)
        {
        }

        public ValidationException(BrokenRulesList brokenRules)
            : this(brokenRules, null)
        {
        }

        public ValidationException(BrokenRulesList brokenRules, Exception? innerException)
            : base(brokenRules?.ToString(), innerException)
        {
            BrokenRules = brokenRules ?? new BrokenRulesList();
        }

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}**Broken Rules**{Environment.NewLine}{BrokenRules}";
        }
    }
}
