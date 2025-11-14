using Common.Core;
using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class UserNameDisplay(string? userName, IName name) : ValueObject<UserNameDisplay>
    {
        public string UserName { get; } = (userName ?? throw new ArgumentNullException(nameof(userName))).SetEmptyToNull()
            ?? throw new ArgumentNullException(nameof(userName));
        public IName Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

        public string Display => $"{Name} ({UserName})";

        public override string ToString()
        {
            return Display;
        }
    }
}
