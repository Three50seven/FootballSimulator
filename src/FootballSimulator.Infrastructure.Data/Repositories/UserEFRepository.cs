using Common.Core;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public class UserEFRepository : FootballSimulatorEntityRepositoryBase<User>, IUserRepository
    {
        public UserEFRepository(FootballSimulatorDbContext context, IEntityHistoryStore store)
            : base(context, store)
        {
        }

        protected override IQueryable<User> EntitySet => base.EntitySet
                                                            .Include(u => u.UserRoles)
                                                                .ThenInclude(ur => ur.Role)
                                                            .Where(u => u.Id != User.SystemUserId); //exclude system user from all user queries - note, this also prevents anyone from logging in as the system user

        public async Task<User?> GetByApplicationIdentityAsync(string userNameOrApplicationUserId, CancellationToken cancellationToken = default, bool includeArchived = false)
        {
            if (includeArchived)
                return await DbSet.Include(u => u.UserRoles)
                            .ThenInclude(ur => ur.Role)
                            .FirstOrDefaultAsync(u => string.Equals(u.UserName, userNameOrApplicationUserId) || string.Equals(u.ApplicationUserGuid, userNameOrApplicationUserId));
            else
                return await EntitySet.FirstOrDefaultAsync(u => string.Equals(u.UserName, userNameOrApplicationUserId) || string.Equals(u.ApplicationUserGuid, userNameOrApplicationUserId));
        }
    }
}
