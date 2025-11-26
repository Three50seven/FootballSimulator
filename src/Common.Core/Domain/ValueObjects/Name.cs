namespace Common.Core.Domain
{
    public class Name : ValueObject<Name>, IName
    {
        protected Name() { }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName.SetEmptyToNull();
            LastName = lastName.SetEmptyToNull();
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public virtual string Initials => string.Concat(FirstName?[0], LastName?[0]);

        public virtual string ToFormalString()
        {
            return $"{LastName}, {FirstName}";
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public static Name Empty => new Name();
    }
}
