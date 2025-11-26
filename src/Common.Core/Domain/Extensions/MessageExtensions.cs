using System.Net.Mail;

namespace Common.Core.Domain
{
    public static class MessageExtensions
    {
        /// <summary>
        /// Convert a message to <see cref="MailMessage"/> for sending email through System.Net.Mail.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="includeAttachments">Whether attachments should be loaded from <see cref="Message.AttachmentPaths"/>. Default is false.</param>
        /// <returns></returns>
        public static MailMessage ToMailMessage(this Message message, bool includeAttachments = false) 
        {
            if (message == null)
                return null;

            var mailMessage = new MailMessage()
            {
                From = message?.From?.ToMailAddress(),
                Subject = string.IsNullOrWhiteSpace(message.Subject) ? string.Empty : message.Subject.Trim(),
                Body = string.IsNullOrWhiteSpace(message.Content) ? string.Empty : message.Content.Trim(),
                IsBodyHtml = message.IsHtml
            };

            if (!message.SendIndividually)
            {
                if (message.ToAddresses != null)
                {
                    foreach (var address in message.ToAddresses)
                    {
                        mailMessage.To.Add(address?.ToMailAddress());
                    }
                }

                if (message.CcAddresses != null)
                {
                    foreach (var address in message.CcAddresses)
                    {
                        mailMessage.CC.Add(address?.ToMailAddress());
                    }
                }

                if (message.BccAddresses != null)
                {
                    foreach (var address in message.BccAddresses)
                    {
                        mailMessage.Bcc.Add(address?.ToMailAddress());
                    }
                }
            }

            if (message.ReplyToAddresses != null)
            {
                foreach (var address in message.ReplyToAddresses)
                {
                    mailMessage.ReplyToList.Add(address?.ToMailAddress());
                }
            }
            
            if (includeAttachments)
            {
                foreach (var path in message.AttachmentPaths)
                {
                    mailMessage.Attachments.Add(new Attachment(path));
                }
            }

            return mailMessage;
        }
    }
}
