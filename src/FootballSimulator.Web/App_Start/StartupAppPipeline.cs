using Common.AspNetCore;
using Common.Core;
using Common.EntityFrameworkCore;
using FootballSimulator.Infrastructure.Data;
using FootballSimulator.Web.Components;

namespace FootballSimulator.Web
{
    internal static class StartupAppPipeline
    {
        public static WebApplication ConfigureAppPipeline(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            // Logging-only step in pipeline.
            // Logs under ILogger<ExceptionLoggingMiddleware>.
            app.UseExceptionLogging(StartupLoggingConfiguration.Options);

            app.Initialize();

            return app;
        }

        private static WebApplication Initialize(this WebApplication app)
        {
            using var scopedServices = app.Services.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;
            var data = serviceProvider.GetRequiredService<FootballSimulatorDbContext>();

            var migrationToLatestConfigValue = app.Configuration["DatabaseSettings:MigrateToLatestVersion"];
            if (migrationToLatestConfigValue != null && !migrationToLatestConfigValue.ParseBool(allowEmpty: true, throwError: false))
                return app;

            // Setup database and be sure latest migrations are applied
            app.Services.InitializeDatabase<FootballSimulatorDbContext>(Seeder.Seed);
            // Add any additional initialization code here in the future.
            return app;
        }
    }
}
