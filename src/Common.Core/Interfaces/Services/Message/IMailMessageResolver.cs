using Common.Core.Domain;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Common.Core
{
    /// <summary>
    /// Resolve <see cref="MailMessage"/> for sending email from a given <see cref="Message"/> record.
    /// </summary>
    public interface IMailMessageResolver
    {
        /// <summary>
        /// Resolve <see cref="MailMessage"/> for sending email from a given <see cref="Message"/> record.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        MailMessage Resolve(Message message);

        /// <summary>
        /// Resolve <see cref="MailMessage"/> for sending email from a given <see cref="Message"/> record.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<MailMessage> ResolveAsync(Message message);
    }
}
