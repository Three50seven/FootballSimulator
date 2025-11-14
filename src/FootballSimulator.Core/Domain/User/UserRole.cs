using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class UserRole : IEntity
    {
        protected UserRole() 
        {
            // Initialize non-nullable navigation properties to null!
            // This suppresses CS8618, but you must ensure they are set before use.
            User = null!;
            Role = null!;
        }

        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
            User = null!;
            Role = null!;
        }
        public UserRole(User user, Role role)
        {
            User = user;
            Role = role;
        }

        public UserRole(User user, int roleId)
        {
            User = user;
            RoleId = roleId;
            Role = null!;
        }

        public int RoleId { get; private set; }
        public int UserId { get; private set; }

        public virtual Role Role { get; private set; }
        public virtual User User { get; private set; }

        public override string ToString()
        {
            return $"Role {RoleId} ({Role?.Name}) for User {UserId} ({User.UserName})";
        }
    }
}
