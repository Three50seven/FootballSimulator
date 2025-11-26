namespace Common.Core.Validation
{
    public class DetailedValidationRule : ValidationRule
    {
        public DetailedValidationRule(string message, string details) 
            : base(message)
        {
            Details = details;
        }

        public string Details { get; }

        public override string ToString()
        {
            return $"{Message} {Details}.";
        }
    }
}
