using Common.Core.Domain;

namespace FootballSimulator.Application.Services

{
    public interface IIdentitySignOnService
    {
        Task<CommandResult> LoginAsync(string userName);
        Task LogoutAsync();
    }
}
