using Common.Core.Domain;

namespace Common.Core.Validation
{
    public class ValidationRule : ValueObject<ValidationRule>
    {
        public string Description { get; private set; }

        public virtual string Message => Description;

        public ValidationRule(string description)
        {
            Description = description.SetNullToEmpty();
        }

        public override string ToString()
        {
            return $"Description: {Description}.";
        }
    }
}
