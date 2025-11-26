using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class UserLoginHistory : Entity<int>
    {
        protected UserLoginHistory()
        {
            Event = new UserCommandEvent(0);
        }
        public UserLoginHistory(int userId, string? ipAddress)
        {
            Event = new UserCommandEvent(userId);
            IpAddress = ipAddress;
        }
        public UserCommandEvent Event { get; private set; }
        public string? IpAddress { get; private set; } = string.Empty;
    }
}
