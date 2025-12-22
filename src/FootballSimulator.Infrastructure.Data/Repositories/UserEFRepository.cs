using Common.Core;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public class UserEFRepository : FootballSimulatorEntityRepositoryBase<User>, IUserRepository
    {
        public UserEFRepository(IDbContextFactory<FootballSimulatorDbContext> factory, IEntityHistoryStore store)
            : base(factory, store)
        {
        }

        protected override IQueryable<User> BuildFullEntitySet(FootballSimulatorDbContext db)
        {
            var query = base.BuildFullEntitySet(db);

            // Always exclude the system user:
            query = query.Where(u => u.Id != User.SystemUserId);

            // Include roles by default:
            query = query.Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role);

            return query;
        }

        public bool CheckForExistingEmail(string? email, int? id)
        {
            using var db = Factory.CreateDbContext();
            return BuildFullEntitySet(db).Where(u => u.Id != id).Any(u => string.Equals(u.Email, email));
        }

        public bool CheckForExistingUserName(string? userName, int? id)
        {
            using var db = Factory.CreateDbContext();
            return BuildFullEntitySet(db).Where(u => u.Id != id).Any(u => string.Equals(u.UserName, userName));
        }

        public async Task<User?> GetByApplicationIdentityAsync(string userNameOrApplicationUserId, CancellationToken cancellationToken = default, bool includeArchived = false)
        {
            using var db = await Factory.CreateDbContextAsync(cancellationToken);
            if (includeArchived)
            {
                return base.BuildFullEntitySet(db)
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefault(u => string.Equals(u.UserName, userNameOrApplicationUserId) || string.Equals(u.ApplicationUserId, userNameOrApplicationUserId));
            }
            else
                return await BuildFullEntitySet(db).FirstOrDefaultAsync(u => string.Equals(u.UserName, userNameOrApplicationUserId) || string.Equals(u.ApplicationUserId, userNameOrApplicationUserId));
        }
    }
}
