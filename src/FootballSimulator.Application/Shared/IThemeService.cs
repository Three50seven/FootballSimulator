namespace FootballSimulator.Application.Services
{
    public interface IThemeService
    {
        public string? CurrentTheme { get; }
        public event Action? OnThemeChanged;
        public Task InitializeAsync();
        public Task ToggleThemeAsync();
    }
}
