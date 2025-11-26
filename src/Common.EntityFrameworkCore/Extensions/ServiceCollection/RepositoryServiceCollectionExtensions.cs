using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.EntityFrameworkCore
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static Func<Type, bool> DefaultRepositoryTypeLookupQuery = t => t.IsClass && !t.IsAbstract && t.Name.Contains("Repository");

        /// <summary>
        /// Adds all application repositories found in the Assembly of the DbContext <typeparamref name="TContext"/> or <paramref name="repositoryAssembly"/>.
        /// All interfaces under a given repository implementation will be registered.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="repositoryAssembly">Optional assembly containing application repositories. Defaults to the DbContext <typeparamref name="TContext"/> assembly.</param>
        /// <param name="entitiesAssembly">Optional assembly containing application core Entities (typically the Core project). Will register all LookupEntities, etc.</param>
        /// <param name="query">
        /// Optional query on which repository types should be returned. Checks for containing "Repository" in name by default. 
        /// For customizing the default query, see <see cref="DefaultRepositoryTypeLookupQuery"/>.
        /// </param>
        /// <param name="overwrite">Optionally overwrite any previously registered type. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories<TContext>(
            this IServiceCollection services,
            Assembly repositoryAssembly = null,
            Assembly entitiesAssembly = null,
            Func<Type, bool> query = null,
            bool overwrite = false)
            where TContext : DbContext
        {
            Guard.IsNotNull(services, nameof(services));

            AddRepositories(services, repositoryAssembly ?? typeof(TContext).Assembly, query, overwrite: overwrite);

            AddLookupRepositories<TContext>(services, entitiesAssembly: entitiesAssembly, overwrite: overwrite);

            services.AddRepository<EntitySlugEFRepository>(overwrite: overwrite);

            return services;
        }

        /// <summary>
        /// Repositories for common, shared entities like <see cref="Message"/>, <see cref="EntityHistory"/>, <see cref="Process"/>, and <see cref="Document"/>.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="overwrite">Optionally overwrite any previously registered type. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddCommonSharedRepositories<TContext>(
            this IServiceCollection services,
            bool overwrite = false)
            where TContext : DbContext, IContextHistorical
        {
            Guard.IsNotNull(services, nameof(services));

            services.AddRepository<EntityHistoryEFRepository<TContext>>(overwrite: overwrite);
            services.AddRepository<MessageEFRepository<TContext>>(overwrite: overwrite);
            services.AddRepository<ProcessEFRepository<TContext>>(overwrite: overwrite);

            AddDocumentRepositories<TContext>(services);

            return services;
        }

        /// <summary>
        /// Adds all application repositories found in the provided assembly <paramref name="assembly"/>.
        /// All interfaces under a given repository implementation will be registered.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="assembly">Assembly containing public repository implementations.</param>
        /// <param name="query">
        /// Optional query on which repository types should be returned. Checks for containing "Repository" in name by default. 
        /// For customizing the default query, see <see cref="DefaultRepositoryTypeLookupQuery"/>.
        /// </param>
        /// <param name="lifetime">Service lifetime. Defaults to Scoped.</param>
        /// <param name="overwrite">Optionally overwrite any previously registered type. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(
            this IServiceCollection services,
            Assembly assembly,
            Func<Type, bool> query = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped,
            bool overwrite = false)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(assembly, nameof(assembly));

            query ??= DefaultRepositoryTypeLookupQuery;

            var types = assembly.GetExportedTypes()
                                .Where(t => t.GetCustomAttribute<DisableAutoServiceRegistrationAttribute>() == null)
                                .Where(t => query.Invoke(t));

            foreach (var type in types)
            {
                services.AddRepository(type, lifetime, overwrite);
            }

            return services;
        }

        /// <summary>
        /// Add repository class to the service registration. All interfaces tied to the repository class will be registered.
        /// Type must be non-abstract class with a name that ends in "Repository".
        /// </summary>
        /// <typeparam name="TRepo">Non-abstract class with a name that contains "Repository".</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="lifetime">Service lifetime. Defaults to Scoped.</param>
        /// <param name="overwrite">Optionally overwrite any previously registered type. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddRepository<TRepo>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped,
            bool overwrite = false)
            where TRepo : class
        {
            return AddRepository(services, typeof(TRepo), lifetime, overwrite);
        }

        /// <summary>
        /// Add repository class to the service registration. All interfaces tied to the repository class will be registered.
        /// Type must be non-abstract class with a name that ends in "Repository".
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="type">
        /// Non-abstract class with a name that contains "Repository". 
        /// For customizing the default query, see <see cref="DefaultRepositoryTypeLookupQuery"/>.
        /// </param>
        /// <param name="lifetime">Service lifetime. Defaults to Scoped.</param>
        /// <param name="overwrite">Optionally overwrite any previously registered type. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddRepository(
            this IServiceCollection services,
            Type type,
            ServiceLifetime lifetime = ServiceLifetime.Scoped,
            bool overwrite = false)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(type, nameof(type));

            if (!DefaultRepositoryTypeLookupQuery.Invoke(type))
                throw new InvalidOperationException($"Cannot register repository of type {type.FullName}. Repository must not be abstract and must end in 'Repository'.");

            return services.AddWithAllInterfaces(type, lifetime, overwrite);
        }

        /// <summary>
        /// Add custom Lookup repository to the service collection.
        /// Uses generic type <see cref="EFLookupRepository{TContextType, TType}"/> and is registered under all associated interfaces.
        /// </summary>
        /// <typeparam name="TEntity">Lookup entity.</typeparam>
        /// <typeparam name="TContext">Application's DbContext</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="lifetime">Service lifetime. Defaults to Scoped.</param>
        /// <param name="overwrite">Optionally overwrite any previously registered type. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddLookupRepository<TEntity, TContext>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped,
            bool overwrite = false)
            where TEntity : class, ILookupEntity
            where TContext : DbContext
        {
            return services?.AddRepository<EFLookupRepository<TContext, TEntity>>(lifetime, overwrite);
        }

        /// <summary>
        /// Register all generic <see cref="ILookupEntity"/> lookup repositories as <see cref="EFLookupRepository{TContextType, TType}"/> 
        /// based on <see cref="ILookupEntity"/> entities found in assembly.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="entitiesAssembly">Optional assembly containing application core Entities (typically the Core project).</param>
        /// <param name="lifetime">ervice lifetime. Defaults to Scoped.</param>
        /// <param name="overwrite">Optionally overwrite any previously registered type. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddLookupRepositories<TContext>(
            this IServiceCollection services,
            Assembly entitiesAssembly = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped,
            bool overwrite = false)
        {
            Guard.IsNotNull(services, nameof(services));

            if (entitiesAssembly == null)
                entitiesAssembly = typeof(TContext).Assembly;

            var lookupTypes = from type in entitiesAssembly.GetExportedTypes()
                              where typeof(ILookupEntity).IsAssignableFrom(type)
                                && type.IsClass
                                && !type.IsAbstract
                              select type;

            foreach (var type in lookupTypes)
            {
                services.AddRepository(typeof(EFLookupRepository<,>).MakeGenericType(typeof(TContext), type), lifetime, overwrite);
            }

            return services;
        }

        /// <summary>
        /// Adds repository registrations for <see cref="Document"/> and <see cref="DocumentDirectory"/>.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDocumentRepositories<TContext>(this IServiceCollection services)
             where TContext : DbContext
        {
            Guard.IsNotNull(services, nameof(services));

            return services.AddRepository<DocumentEFRepository<TContext>>()
                           .AddRepository<DocumentDirectoryEFRepository<TContext>>();
        }
    }
}
