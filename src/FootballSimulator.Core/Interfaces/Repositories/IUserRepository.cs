using Common.Core;
using FootballSimulator.Core.Domain;

namespace FootballSimulator.Core.Interfaces
{
    public interface IUserRepository : IDomainRepository<User>
    {
        Task<User?> GetByApplicationIdentityAsync(string userNameOrApplicationUserId, CancellationToken cancellationToken = default, bool includeArchived = false);
    }
}
