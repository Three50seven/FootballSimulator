namespace FootballSimulator.Core.Interfaces
{
    public interface IUserLoginRecorder
    {
        Task RecordAsync(int userId, string ipAddress);
    }
}
