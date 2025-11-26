using Common.Core;
using Common.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.EntityFrameworkCore
{
    public static class DbContextServiceCollectionExtensions
    {
        public const string DefaultConnStringName = "DefaultConnection";

        /// <summary>
        /// Registers <typeparamref name="TContext"/> as a DbContext on the service collection.
        /// Uses connection string name <paramref name="connStringName"/> found in configuration.
        /// When using EF Core 5, an implementation of IDbContextFactory will be registered to used to resolve the DbContext.
        /// </summary>
        /// <typeparam name="TContext">Application DbContext type.</typeparam>
        /// <param name="services">Existing services collection.</param>
        /// <param name="configuration">Required configuration from the executing application.</param>
        /// <param name="connStringName">Name of the connection string found in the ConnectionStrings section of <see cref="IConfiguration"/>.</param>
        /// <param name="buildOptions">Optional callback for building DbContext options.</param>
        /// <param name="sqlBuildOptions">Optional callback for building the SqlServer DbContext options.</param>
        /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container. Should almost always be scoped.</param>
        /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container. Defaults to singleton to allow for use in singleton services (like a factory).</param>
        /// <param name="registerAsDefaultContext">Whether this DbContext should be registered for the generic base items <see cref="DbContext"/> and <see cref="IUnitOfWork"/>. Defaults to true.</param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseContext<TContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            string connStringName = DefaultConnStringName,
            Action<DbContextOptionsBuilder>? buildOptions = null,
            Action<SqlServerDbContextOptionsBuilder>? sqlBuildOptions = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Singleton,
            bool registerAsDefaultContext = true)
            where TContext : DbContextBase<TContext>
        {
            AddDbContext<TContext>(services, configuration, connStringName, buildOptions, sqlBuildOptions, contextLifetime, optionsLifetime, registerAsDefaultContext);

            services.AddScoped<DbContextBase<TContext>>(serviceProvider => serviceProvider.GetRequiredService<TContext>());

            return services;
        }

        /// <summary>
        /// Registers <typeparamref name="TContext"/> as a DbContext on the service collection.
        /// Uses connection string name <paramref name="connStringName"/> found in configuration.
        /// When using EF Core 5, an implementation of IDbContextFactory will be registered to used to resolve the DbContext.
        /// </summary>
        /// <typeparam name="TContext">Application DbContext type.</typeparam>
        /// <param name="services">Existing services collection.</param>
        /// <param name="configuration">Required configuration from the executing application.</param>
        /// <param name="connStringName">Name of the connection string found in the ConnectionStrings section of <see cref="IConfiguration"/>.</param>
        /// <param name="buildOptions">Optional callback for building DbContext options.</param>
        /// <param name="sqlBuildOptions">Optional callback for building the SqlServer DbContext options.</param>
        /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container. Should almost always be scoped.</param>
        /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container. Defaults to singleton to allow for use in singleton services (like a factory).</param>
        /// <param name="registerAsDefaultContext">Whether this DbContext should be registered for the generic base items <see cref="DbContext"/> and <see cref="IUnitOfWork"/>. Defaults to true.</param>
        /// <returns></returns>
        public static IServiceCollection AddDbContext<TContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            string connStringName = DefaultConnStringName,
            Action<DbContextOptionsBuilder>? buildOptions = null,
            Action<SqlServerDbContextOptionsBuilder>? sqlBuildOptions = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Singleton,
            bool registerAsDefaultContext = true)
            where TContext : DbContext, IUnitOfWork
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(configuration, nameof(configuration));
            Guard.IsNotNull(connStringName, nameof(connStringName));

            // include factory and resolve DbContext from that
            services.AddDbContextFactory<TContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connStringName), sqlBuildOptions);
                buildOptions?.Invoke(options);
            },
            lifetime: optionsLifetime);

            // register the DbContext itself and use factory to resolve it
            services.Add(new ServiceDescriptor(
                typeof(TContext),
                serviceProvider => serviceProvider.GetRequiredService<IDbContextFactory<TContext>>().CreateDbContext(),
                lifetime: contextLifetime));

            if (registerAsDefaultContext)
            {
                services.AddScoped<DbContext>(serviceProvider => serviceProvider.GetRequiredService<TContext>());
                services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<TContext>());
            }

            return services;
        }
    }
}
