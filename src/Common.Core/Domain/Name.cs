namespace Common.Core.Domain
{
    public class Name : ValueObject<Name>, IName
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public virtual string Initials => string.Concat(FirstName?[0], LastName?[0]);

        public static Name Empty => new Name();

        protected Name()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName?.SetEmptyToNull() ?? string.Empty;
            LastName = lastName?.SetEmptyToNull() ?? string.Empty;
        }

        public virtual string ToFormalString()
        {
            return $"{LastName}, {FirstName}";
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
