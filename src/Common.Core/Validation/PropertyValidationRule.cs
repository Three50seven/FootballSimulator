namespace Common.Core.Validation
{
    public class PropertyValidationRule : ValidationRule
    {
        public string Name { get; private set; }

        public PropertyValidationRule(string name, string description)
            : base(description)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Name: {Name}. Description: {base.Description}.";
        }
    }
}
