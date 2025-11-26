using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Core.Services
{
    /// <summary>
    /// Base class for any service-like definition that is typically registered as a scoped dependency
    /// to allow for a out-of-scope Singleton version that will create the scope per method call.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public abstract class ScopedAdapterBase<TService>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedAdapterBase(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            ValidateRegisteredServices(_serviceScopeFactory);
        }

        /// <summary>
        /// Validates registered services. This is called from the constructor to help validate as early as possible.
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        protected virtual void ValidateRegisteredServices(IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = CreateScope())
            {
                GetService(scope);
            }
        }

        /// <summary>
        /// Helper function to get the generic service type from the service provider.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected static TService GetService(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<TService>();

        /// <summary>
        /// Create a service scope that should be deposed immediately after use.
        /// </summary>
        /// <returns></returns>
        protected virtual IServiceScope CreateScope() => _serviceScopeFactory.CreateScope();
    }
}