using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class Role : LookupEntity
    {
        private Role() { }

        public Role(RoleOption role)
            : base(role) { }
    }
}
