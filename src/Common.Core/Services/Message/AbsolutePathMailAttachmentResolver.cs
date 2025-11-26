using Common.Core.Domain;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Default mail attachment resolver. Uses absolutely path via <see cref="PathHelper.GetAbsolutePath(string, string)"/> to resolve a path from a given path.
    /// Given path designed to be from <see cref="MessageAttachment.Document"/> full path.
    /// </summary>
    public class AbsolutePathMailAttachmentResolver : IMailAttachmentResolver
    {
        public AbsolutePathMailAttachmentResolver(string root)
        {
            if (string.IsNullOrWhiteSpace(root))
                throw new ArgumentException($"A Root absolute file path value must be provided.");

            Root = root.Trim();
        }

        protected string Root { get; }

        public Attachment Resolve(string path)
        {
            return new Attachment(PathHelper.GetAbsolutePath(path, Root));
        }

        public Task<Attachment> ResolveAsync(string path)
        {
            var attachment = Resolve(path);
            return Task.FromResult(attachment);
        }
    }
}
