namespace Common.Core.Validation
{
    public class PropertyValidationRule : ValidationRule
    {
        public PropertyValidationRule(string name, string description) : base(description)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return $"Name: {Name}. Description: {Description}.";
        }
    }
}
