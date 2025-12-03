using FootballSimulator.Application.Services;
using Microsoft.JSInterop;

namespace FootballSimulator.Web.Providers
{
    public class ThemeService : IThemeService
    {
        private readonly IJSRuntime _js;
        public string CurrentTheme { get; private set; } = "dark";

        public event Action? OnThemeChanged;

        public ThemeService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var savedTheme = await _js.InvokeAsync<string>("localStorage.getItem", "theme");
                if (string.IsNullOrEmpty(savedTheme))
                {
                    savedTheme = "dark";
                }
                CurrentTheme = savedTheme;
                // The JS themeManager already applied it on page load
            }
            catch (Exception)
            {
                CurrentTheme = "dark";
            }
        }

        public async Task ToggleThemeAsync()
        {
            var newTheme = CurrentTheme == "dark" ? "light" : "dark";
            CurrentTheme = newTheme;
            await _js.InvokeVoidAsync("setBodyThemeClass", newTheme);
            OnThemeChanged?.Invoke();
        }
    }
}