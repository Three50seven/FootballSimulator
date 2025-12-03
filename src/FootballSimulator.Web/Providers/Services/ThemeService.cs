using Microsoft.JSInterop;

namespace FootballSimulator.Web
{
    public class ThemeService
    {
        private readonly IJSRuntime _js;
        public string CurrentTheme { get; private set; } = "dark";

        public ThemeService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task InitializeAsync()
        {
            var savedTheme = await _js.InvokeAsync<string>("localStorage.getItem", "theme");
            if (string.IsNullOrEmpty(savedTheme))
            {
                savedTheme = "dark";
                await _js.InvokeVoidAsync("localStorage.setItem", "theme", savedTheme);
            }

            CurrentTheme = savedTheme;
            await ApplyThemeAsync(savedTheme);
        }

        public async Task ApplyThemeAsync(string theme)
        {
            CurrentTheme = theme;
            await _js.InvokeVoidAsync("document.body.setAttribute", "data-bs-theme", theme);
        }

        public async Task ToggleThemeAsync()
        {
            var newTheme = CurrentTheme == "dark" ? "light" : "dark";
            await _js.InvokeVoidAsync("localStorage.setItem", "theme", newTheme);
            await ApplyThemeAsync(newTheme);
        }
        public async Task ReapplyThemeAsync()
        {
            // Always apply the current theme
            await _js.InvokeVoidAsync("document.body.setAttribute", "data-bs-theme", CurrentTheme);
        }
    }
}