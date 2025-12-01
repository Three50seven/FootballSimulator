using Common.Core;
using Common.Core.Domain;
using Common.Core.Services;
using Common.Data;
using Common.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FootballSimulator.Infrastructure.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityDatabaseContext<FootballSimulatorDbContext>(configuration, buildOptions: options =>
            {
#if !DEBUG
                // Ref - https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager#single-and-split-queries
                options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.MultipleCollectionIncludeWarning));
#endif
            });

            // scan for standard repositories
            services.AddRepositories<FootballSimulatorDbContext>(query: t => t.IsClass
                                                              && !t.IsAbstract
                                                              && t.Name.Contains("Repository"));


            services.AddCommonSharedRepositories<FootballSimulatorDbContext>();

            // default dependencies to ensure class validation - the running applications will most likely overwrite these
            services.TryAddScoped<IEntityHistoryStore, EntityHistoryStore>();
            services.TryAddSingleton<IUserId, DefaultUser>();

            return services;
        }

        public static IServiceCollection AddSqlClientDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSqlConnectionFactory(configuration);
        }        
    }
}
