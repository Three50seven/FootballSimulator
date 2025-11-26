namespace Common.Core.Domain
{
    public class FullName : Name
    {
        protected FullName()
        {
        }

        public FullName(
            string firstName, 
            string lastName, 
            string middleName = null, 
            string prefix = null, 
            string suffix = null, 
            string nickname = null)
            : base(firstName, lastName)
        {
            MiddleName = middleName.SetEmptyToNull();
            Prefix = prefix.SetEmptyToNull();
            Suffix = suffix.SetEmptyToNull();
            Nickname = nickname.SetEmptyToNull();
        }

        public string MiddleName { get; private set; }
        public string Suffix { get; private set; }
        public string Prefix { get; private set; }
        public string Nickname { get; private set; }

        public string MiddleInitial => string.IsNullOrWhiteSpace(MiddleName) ? null : MiddleName[0].ToString();

        public override string ToFormalString()
        {
            return string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName}";
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
        }

        public static new FullName Empty => new FullName();
    }
}
