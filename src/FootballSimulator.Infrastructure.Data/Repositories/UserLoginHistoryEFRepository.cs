using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;

namespace FootballSimulator.Infrastructure.Data
{
    public class UserLoginHistoryEFRepository : FootballSimulatorRepositoryBase<UserLoginHistory, int>, IUserLoginHistoryRepository
    {
        public UserLoginHistoryEFRepository(FootballSimulatorDbContext context) : base(context)
        {
        }

        protected override IQueryable<UserLoginHistory> EntitySet => base.EntitySet;

        public DateTime? GetLastLoginDate(int userId)
        {
            var userHistory = EntitySet.Where(u => u.Event.UserId == userId).OrderByDescending(o => o.Event.Date).FirstOrDefault();

            if (userHistory! == null!)
                return null;

            return userHistory.Event.Date;
        }
    }   
}