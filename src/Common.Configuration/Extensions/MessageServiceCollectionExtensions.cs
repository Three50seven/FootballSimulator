using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.Configuration;
using Common.Core;
using Common.Core.Domain;
using Common.Core.Services;
using Common.Core.Validation;

namespace Common.Configuration
{
    public static class MessageServiceCollectionExtensions
    {
        /// <summary>
        /// Add basic queuing and sending services in <see cref="Common.Core"/> for the Message entity <see cref="Message"/>.
        /// Configuration settings are read for <see cref="MessageSettings"/> under the provided <paramref name="sectionName"/>.
        /// </summary>
        /// <param name="services">Existing services collection.</param>
        /// <param name="configuration">Established configuration from the executing application.</param>
        /// <param name="sectionName">Custom configuration settings name for <see cref="MessageSettings"/>. Defaults to "MessageSettings".</param>
        /// <returns></returns>
        public static IServiceCollection AddMessages(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = "MessageSettings")
        {
            Guard.IsNotNull(services, nameof(services));

            var section = configuration.GetSection(sectionName, required: true);
            var messageSettings = new MessageSettings();
            section.Bind(messageSettings);

            return services.AddMessages(messageSettings);
        }
    }
}
