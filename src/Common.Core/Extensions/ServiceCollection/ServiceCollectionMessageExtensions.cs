using Microsoft.Extensions.DependencyInjection;
using Common.Core.Domain;
using Common.Core.DTOs;
using Common.Core.Services;
using Common.Core.Validation;
using System.IO;
using System.Net.Mail;

namespace Common.Core
{
    public static class ServiceCollectionMessageExtensions
    {
        /// <summary>
        /// Add basic queuing and sending services in Core library for the default Message entity <see cref="Message"/> and options <see cref="MessageOptions"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="settings">Settings to determine if and how messages are queued/sent from the application.</param>
        /// <returns></returns>
        public static IServiceCollection AddMessages(this IServiceCollection services, MessageSettings settings)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(settings, nameof(settings));

            services.AddSingleton<MessageSettings>(settings);

            switch (settings.SendType)
            {
                case MessageSendingOption.Smtp:
                    services.AddScoped<IMessageSender, SystemNetMessageSender>();
                    break;
                case MessageSendingOption.WriteToFile:
                    services.AddSingleton<IMessageSender, MessageSaveToFileSender>();
                    break;
                default:
                    throw new UnsupportedEnumException(settings.SendType);
            }

            switch (settings.QueueType)
            {
                case MessageQueueOption.DatabaseOnly:
                    services.AddScoped<IMessageService, MessageDatabaseOnlyService>();
                    break;
                case MessageQueueOption.DirectSend:
                    services.AddScoped<IMessageService, MessageDirectSendingService>();
                    break;
                case MessageQueueOption.QueueBackgroundTask:
                    services.AddScoped<IMessageService, MessageTaskQueuingService>();
                    break;
                default:
                    throw new UnsupportedEnumException(settings.QueueType);
            }

            
            services.AddScoped<MessageBackgroundTaskSender>();            

            services.AddSingleton<IMailMessageResolver, DefaultMailMessageResolver>();
            services.AddSingleton<IMailAttachmentResolver>((serviceProvider) =>
            {
                string root = !string.IsNullOrWhiteSpace(settings.AttachmentsDirectory) ? settings.AttachmentsDirectory
                              : Path.Combine(Directory.GetCurrentDirectory(), "mail_attachments");

                return new AbsolutePathMailAttachmentResolver(root);
            });

            services.AddScoped<SmtpClient>((serviceProvider) => serviceProvider.GetRequiredService<MessageSettings>().Smtp?.ToSmtpClient());
            services.AddSingleton<MessageValidatingInfo>((serviceProvider) => serviceProvider.GetRequiredService<MessageSettings>().ToValidatingInfo());

            return services;
        }
    }
}