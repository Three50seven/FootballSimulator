using Microsoft.Extensions.DependencyInjection;
using Common.Core.Services;
using Common.Core.Validation;

namespace Common.Core
{
    public static class ServiceCollectionDocumentExtensions
    {
        /// <summary>
        /// Adds Document upload and file saving services for uploading files and storing file information in database using <see cref="Domain.Document"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddDocumentServices(this IServiceCollection services)
        {
            Guard.IsNotNull(services, nameof(services));

            services.AddSingleton<ISubPathCreator, EmptySubPathCreator>();
            services.AddScoped<IDocumentUploadService, DocumentUploadService>();

            return services;
        }
    }
}
