using Microsoft.Extensions.Logging;
using Common.Core.Domain;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Detault implmentation of <see cref="IMessageSender"/> for sending <see cref="Message"/> records as email over SMTP.
    /// This sender class is used when <see cref="MessageSettings.SendType"/> is <see cref="MessageSendingOption.Smtp"/>.
    /// </summary>
    public class SystemNetMessageSender : MessageSenderBase<SystemNetMessageSender>
    {
        public SystemNetMessageSender(
            SmtpClient client,
            IMailMessageResolver mailMessageResolver,
            ILogger<SystemNetMessageSender> logger)
            : base(logger)
        {
            Client = client;
            MailMessageResolver = mailMessageResolver;
        }

        protected SmtpClient Client { get; }
        protected IMailMessageResolver MailMessageResolver { get; }

        protected override void OnSend(Message message)
        {
            using (var msg = MailMessageResolver.Resolve(message))
            {
                if (message.SendIndividually)
                    SendIndividually(message, msg);
                else
                    Client.Send(msg);
            }
        }

        protected override async Task OnSendAsync(Message message)
        {
            using (var mailMessage = await MailMessageResolver.ResolveAsync(message))
            {
                if (message.SendIndividually)
                    await SendIndividuallyAsync(message, mailMessage);
                else
                    await Client.SendMailAsync(mailMessage);
            }
        }

        protected virtual void SendIndividually(Message message, MailMessage mailMessage)
        {
            foreach (var address in message.ToAddresses)
            {
                mailMessage.To.Clear();
                mailMessage.To.Add(address?.ToMailAddress());

                try
                {
                    Client.Send(mailMessage);

                    message.ToRecipients?.SingleOrDefault(r => r.Address.Email == address.Email)?.Complete();
                }
                catch (Exception ex)
                {
                    message.ToRecipients?.SingleOrDefault(r => r.Address.Email == address.Email)?.Fail(ex?.GetFriendlyMessage());

                    // log and continue the rest of the list upon failure
                    LogError(ex);
                }
            }
        }

        protected virtual async Task SendIndividuallyAsync(Message message, MailMessage mailMessage)
        {
            foreach (var address in message.ToAddresses)
            {
                mailMessage.To.Clear();
                mailMessage.To.Add(address?.ToMailAddress());

                try
                {
                    await Client.SendMailAsync(mailMessage);
                    message.ToRecipients?.SingleOrDefault(r => r.Address.Email == address.Email)?.Complete();
                }
                catch (Exception ex)
                {
                    message.ToRecipients?.SingleOrDefault(r => r.Address.Email == address.Email)?.Fail(ex?.GetFriendlyMessage());

                    // log and continue the rest of the list upon failure
                    LogError(ex);
                }
            }
        }
    }
}
