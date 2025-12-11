namespace FootballSimulator.Application.Services
{
    public interface IUserManageModelService
    {
        Task<UserManageModel> BuildModelAsync();
    }
}
