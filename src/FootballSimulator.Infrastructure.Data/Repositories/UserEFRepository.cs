using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.DTOs;
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

        public async Task<IPagedEnumerable<User>> SearchAsync(UserSearchFilter filter, ResultListFilter resultFilter)
        {
            Guard.IsNotNull(filter, nameof(filter));
            Guard.IsNotNull(resultFilter, nameof(resultFilter));

            filter.Clean();

            using var db = await Factory.CreateDbContextAsync();
            IQueryable<User> query = BuildFullEntitySet(db);
            if (filter.FirstName != null)
            {
                query = query.Where(u => u.Name.FirstName.ToLower().Contains(filter.FirstName));
            }
            if (filter.LastName != null)
            {
                query = query.Where(u => u.Name.LastName.ToLower().Contains(filter.LastName));
            }
            if (filter.UserName != null)
            {
                query = query.Where(u => u.UserName != null && u.UserName.ToLower().Contains(filter.UserName));
            }
            if (filter.Email != null)
            {
                query = query.Where(u => u.Email != null && u.Email.ToLower().Contains(filter.Email));
            }

            var orderedQuery = resultFilter.Sorting.SortBy switch
            {
                "Name" => query.OrderBy(u => u.Name.FirstName, resultFilter.Sorting.Direction)
                    .ThenBy(u => u.Name.LastName, resultFilter.Sorting.Direction),
                _ => query.OrderBy(resultFilter.Sorting)
            };

            var results = await orderedQuery.Page(resultFilter.Paging, out int totalCount).ToListAsync();

            return new PagedList<User>(results, totalCount);
        }
    }
}
