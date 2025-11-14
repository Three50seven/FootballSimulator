using Common.Core;
using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    [StoreHistory((int)EntityTypeOption.User, includePropChanges: true)]
    public class User : DomainEntity, IUser, IArchivable
    {
        public const int SystemUserId = 1;
        protected User() 
        {
            Name = new Name(string.Empty, string.Empty);
        }
        public User(string userName, string email, Name name)
        {
            UserName = userName;
            Email = email.SetEmptyToNull();
            Name = name;
        }
                
        public bool Archive { get; set; }
        public string? UserName { get; private set; }
        public string? Email { get; private set; }
        public Name Name { get; private set; }
        public UserNameDisplay NameDisplay => new UserNameDisplay(UserName, Name);

        [TrackChanges]
        public IEnumerable<UserRole> UserRoles { get; private set; } = new List<UserRole>();

        public void RemoveRoles()
        {
            ((List<UserRole>)UserRoles).Clear();
        }

        public void SetUserRoles(IEnumerable<UserRole> userRoles)
        {
            RemoveRoles();

            foreach (var role in userRoles)
            {
                ((List<UserRole>)UserRoles).Add(new UserRole(this, role.RoleId));
            }
        }
    }
}
