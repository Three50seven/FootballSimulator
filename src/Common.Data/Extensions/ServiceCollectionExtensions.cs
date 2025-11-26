using Common.Core;
using Common.Core.Domain;
using Common.Core.Services;
using Common.Core.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Common.Data
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Set <see cref="IFileStorage"/> for storage in a database under <see cref="DatabaseFileStorage"/> and <see cref="FileSourceSqlRepository"/>.
        /// Uses connection string from configuration under the name <paramref name="connStringName"/>.
        /// Please view <see cref="FileSourceSqlRepository"/> and <see cref="FileSource"/> for required schema details.
        /// Be sure indexes and unique constraints are present in schema, particularly for <see cref="FileSource.Path"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="connStringName">Connection string name found in ConnectionStrings section of configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseFileStorage(
            this IServiceCollection services, string connStringName = "FilesDatabaseConnection")
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(connStringName, nameof(connStringName));

            services.TryAddSingleton<IFileSourceRepository>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var dbConnFactory = new SqlDbConnectionFactory(configuration.GetConnectionString(connStringName)!);
                return new FileSourceSqlRepository(dbConnFactory);
            });

            services.TryAddSingleton<FileStorageSettings>(new FileStorageSettings());
            services.AddSingleton<IFileStorage, DatabaseFileStorage>();

            return services;
        }

        /// <summary>
        /// Registers <see cref="IDbConnectionFactory"/> under the connecting string found in configuration
        /// under the name <paramref name="connStringName"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="configuration">Configuration of the running application.</param>
        /// <param name="connStringName">Connection string name found in ConnectionStrings section of configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlConnectionFactory(
            this IServiceCollection services,
            IConfiguration configuration,
            string connStringName = "DefaultConnection")
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(configuration, nameof(configuration));
            Guard.IsNotNull(connStringName, nameof(connStringName));

            return AddSqlConnectionFactory(services, configuration.GetConnectionString(connStringName)!);
        }

        /// <summary>
        /// Registers <see cref="IDbConnectionFactory"/> under the supplied <paramref name="connectionString"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="connectionString">Full connection string used to create the SqlConnection.</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlConnectionFactory(
           this IServiceCollection services,
           string connectionString)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(connectionString, nameof(connectionString));

            services.AddSingleton<IDbConnectionFactory>(serviceProvider =>
            {
                return new SqlDbConnectionFactory(connectionString);
            });

            return services;
        }
    }
}
