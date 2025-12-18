namespace FootballSimulator.Application.Services
{
    public interface IStadiumManageModelService
    {
        Task<StadiumManageModel> BuildModelAsync();
    }
}
