using Common.Core;
using Common.Core.Domain;
using FootballSimulator.Core.Domain;
using FootballSimulator.Core.DTOs;

namespace FootballSimulator.Core.Interfaces
{
    public interface IUserRepository : IDomainRepository<User>
    {
        Task<User?> GetByApplicationIdentityAsync(string userNameOrApplicationUserId, CancellationToken cancellationToken = default, bool includeArchived = false);
        bool CheckForExistingUserName(string? userName, int? id);
        bool CheckForExistingEmail(string? email, int? id);
        Task<IPagedEnumerable<User>> SearchAsync(UserSearchFilter filter, ResultListFilter resultFilter);
    }
}
