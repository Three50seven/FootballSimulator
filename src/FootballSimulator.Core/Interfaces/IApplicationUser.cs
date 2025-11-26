using Common.Core.Domain;
using FootballSimulator.Core.Domain;

namespace FootballSimulator.Core.Interfaces
{
    public interface IApplicationUser : IUser, IUserId
    {
        int UserId { get; }
        Guid UserGuid { get; }
        string UserName { get; }
        UserName Name { get; }
        bool IsLoggedIn { get; }
        bool IsAdmin { get; }
        bool IsInRole(params RoleOption[] roles);
    }
}
