using Common.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.EntityFrameworkCore
{
    public static class DbContextServiceProviderExtensions
    {
        /// <summary>
        /// Initialize a <typeparamref name="TContext"/>, typically at startup of an application.
        /// <paramref name="serviceProvider"/> will create a temporary scope and call <see cref="RelationalDatabaseFacadeExtensions.Migrate"/>.
        /// Optionally supply a callback action <paramref name="seedingEvent"/> to call after migration to seed the database with initial values.
        /// </summary>
        /// <typeparam name="TContext">DbContext type to initialize.</typeparam>
        /// <param name="serviceProvider">Existing service provider.</param>
        /// <param name="seedingEvent">Optional callback for seeding database.</param>
        public static void InitializeDatabase<TContext>(
            this IServiceProvider serviceProvider,
            Action<TContext, IServiceProvider>? seedingEvent = null)
            where TContext : DbContext
        {
            Guard.IsNotNull(serviceProvider, nameof(serviceProvider));

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                context.Database.Migrate();

                if (seedingEvent != null)
                    seedingEvent.Invoke(context, scope.ServiceProvider);
            }
        }

        /// <summary>
        /// Initialize a <typeparamref name="TContext"/>, typically at startup of an application.
        /// <paramref name="serviceProvider"/> will create a temporary scope and call <see cref="RelationalDatabaseFacadeExtensions.MigrateAsync"/>.
        /// Optionally supply a callback action <paramref name="seedingEvent"/> to call after migration to seed the database with initial values.
        /// </summary>
        /// <typeparam name="TContext">DbContext type to initialize.</typeparam>
        /// <param name="serviceProvider">Existing service provider.</param>
        /// <param name="seedingEvent">Optional callback for seeding database.</param>
        /// <returns></returns>
        public static async Task InitializeDatabaseAsync<TContext>(
            this IServiceProvider serviceProvider,
            Func<TContext, IServiceProvider, Task> seedingEvent)
            where TContext : DbContext
        {
            Guard.IsNotNull(serviceProvider, nameof(serviceProvider));

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                await context.Database.MigrateAsync();

                if (seedingEvent != null)
                    await seedingEvent(context, scope.ServiceProvider);
            }
        }
    }
}
