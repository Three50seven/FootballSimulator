using Common.Core.Domain;
using Common.Core.Validation;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Default resolve for resolving <see cref="MailMessage"/> for sending email from a given <see cref="Message"/> record.
    /// Uses <see cref="MessageExtensions.ToMailMessage(Message, bool)"/> to build Mail Message and resolves attachments
    /// using <see cref="IMailAttachmentResolver"/> implementation.
    /// </summary>
    public class DefaultMailMessageResolver : IMailMessageResolver
    {
        public DefaultMailMessageResolver(IMailAttachmentResolver attachmentResolver)
        {
            AttachmentResolver = attachmentResolver;
        }

        protected IMailAttachmentResolver AttachmentResolver { get; }

        public MailMessage Resolve(Message message)
        {
            Guard.IsNotNull(message, nameof(message));

            var mailMessage = message.ToMailMessage();

            foreach (var path in message.AttachmentPaths)
            {
                mailMessage.Attachments.Add(AttachmentResolver.Resolve(path));
            }

            return mailMessage;
        }

        public async Task<MailMessage> ResolveAsync(Message message)
        {
            var mailMessage = message.ToMailMessage();

            foreach (var path in message.AttachmentPaths)
            {
                mailMessage.Attachments.Add(await AttachmentResolver.ResolveAsync(path));
            }

            return mailMessage;
        }
    }
}
