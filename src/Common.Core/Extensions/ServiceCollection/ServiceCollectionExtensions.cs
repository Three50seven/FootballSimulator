using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Common.Core.Domain;
using Common.Core.Interfaces;
using Common.Core.Services;
using Common.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers default common services found in <see cref="Common.Core"/>. 
        /// Includes parsers, local OS, filestorage, entityhistory, etc.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommonCoreServices(this IServiceCollection services)
        {
            Guard.IsNotNull(services, nameof(services));

            services.AddSingleton<IValueParser, ValueParser>();

            AddLocalSystemFileStorage(services);

            services.AddSingleton<IUser, DefaultUser>();
            services.AddSingleton<IUserId>((serviceProvider) => serviceProvider.GetRequiredService<IUser>());

            services.AddSingleton<IPasswordGenerator, RandomPasswordGenerator>();

            return services;
        }

        /// <summary>
        /// Register standard local, OS-level file storage and validator.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="absolute">
        /// Whether or not absolute file storage should be used. Requires a root directory setting. 
        /// Defaults to false in order to use relative paths directly.
        /// </param>
        /// <param name="storageSettings">Optional settings.</param>
        /// <returns></returns>
        public static IServiceCollection AddLocalSystemFileStorage(
            this IServiceCollection services, 
            bool absolute = false,
            FileStorageSettings storageSettings = null)
        {
            services.AddSingleton<FileStorageSettings>(storageSettings ?? new FileStorageSettings());

            if (absolute)
                services.AddSingleton<IFileSystemPathResolver, AbsoluteFileSystemPathResolver>();
            else
                services.AddSingleton<IFileSystemPathResolver, RelativeFileSystemPathResolver>();

            services.AddSingleton<IFileStorage, LocalSystemFileStorage>();
            services.AddSingleton<IFileDirectoryStorage, LocalSystemFileStorage>();

            AddFileValidator(services);

            return services;
        }

        /// <summary>
        /// Register file validator with optional settings.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="storageSettings"></param>
        /// <returns></returns>
        public static IServiceCollection AddFileValidator(
            this IServiceCollection services,
            FileStorageSettings storageSettings = null)
        {
            services.TryAddSingleton<FileStorageSettings>(storageSettings ?? new FileStorageSettings());

            services.AddSingleton<IFileValidator, FileValidator>();
            services.AddSingleton<IReadOnlyDictionary<string, IFileContentValidator>>(serviceProvider =>
            {
                return serviceProvider.GetServices<IFileContentValidator>()?
                                      .ToDictionary(fcv => fcv.Extension) ?? new Dictionary<string, IFileContentValidator>();
            });

            return services;
        }

        /// <summary>
        /// Adds implementation for <see cref="IEntityHistoryStore"/> to the collection.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddEntityHistoryStore(this IServiceCollection services)
        {
            Guard.IsNotNull(services, nameof(services));

            services.TryAddSingleton<IUserId, DefaultUser>();

            services.AddScoped<IEntityHistoryStore, EntityHistoryStore>();

            return services;
        }

        /// <summary>
        /// Register core library services that require dependencies like repositories, current user, and serialization.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoreServicesWithDependencies(this IServiceCollection services)
        {
            AddEntityHistoryStore(services);

            services.AddScoped(typeof(IEntityCopyService<>), typeof(EntityCopyService<>));
            services.AddScoped<ISlugGenerator, SlugGenerator>();

            return services;
        }

        /// <summary>
        /// Add class to the service registration. All interfaces tied to the class will be registered with the class as the implementation.
        /// </summary>
        /// <typeparam name="TRepo">Registerable class type.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="lifetime">Service lifetime. Defaults to Scoped.</param>
        /// <param name="overwrite">Optionally overwrite any previously registered type. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddWithAllInterfaces<TRepo>(
            this IServiceCollection services, 
            ServiceLifetime lifetime = ServiceLifetime.Scoped,
            bool overwrite = false)
            where TRepo : class
        {
            return AddWithAllInterfaces(services, typeof(TRepo), lifetime, overwrite);
        }

        /// <summary>
        /// Add class to the service registration. All interfaces tied to the class will be registered with the class as the implementation.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="type">Registerable class type.</param>
        /// <param name="lifetime">Service lifetime. Defaults to Scoped.</param>
        /// <param name="overwrite">Optionally overwrite any previously registered type. If false, services.TryAdd is used, otherwise services.Add is used.</param>
        /// <returns></returns>
        public static IServiceCollection AddWithAllInterfaces(
            this IServiceCollection services, 
            Type type, 
            ServiceLifetime lifetime = ServiceLifetime.Scoped, 
            bool overwrite = false)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(type, nameof(type));

            if (overwrite)
                services.Add(new ServiceDescriptor(type, type, lifetime));
            else
                services.TryAdd(new ServiceDescriptor(type, type, lifetime));

            var interfaces = type.GetInterfaces();
            if (interfaces != null && interfaces.Any())
                services.AddTypeMatches(interfaces.Select(i => new TypeRegistrationMatch(i, type)), lifetime, overwrite: overwrite);

            return services;
        }
    }
}