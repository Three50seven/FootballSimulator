using System.Net.Mail;
using System.Threading.Tasks;

namespace Common.Core
{
    /// <summary>
    /// Resolve attachments for an email message given a relative path.
    /// </summary>
    public interface IMailAttachmentResolver
    {
        /// <summary>
        /// Resolve attachments for an email message given a relative path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Attachment Resolve(string path);

        /// <summary>
        /// Resolve attachments for an email message given a relative path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Attachment> ResolveAsync(string path);
    }
}
