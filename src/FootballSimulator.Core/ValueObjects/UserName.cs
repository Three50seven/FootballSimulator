using Common.Core;
using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class UserName : ValueObject<UserName>, IComparable, IName
    {
        protected UserName()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public UserName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Initials => string.Concat(FirstName.FirstOrDefault(), LastName.FirstOrDefault());

        public int CompareTo(object? obj)
        {
            return ToFormalString().CompareTo(((UserName)obj!).ToFormalString());
        }

        public string ToFormalString()
        {
            if (this == Empty)
                return string.Empty;
            return $"{LastName}, {FirstName}";
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public static readonly UserName Empty = new();
    }
}
