using System;
using System.Collections.Generic;

namespace Common.Core.Domain
{
    public class FileExtension : ValueObject<FileExtension>
    {
        public FileExtension(string extension, string mimeType, IEnumerable<byte[]> signature = null)
        {
            Extension = extension?.Replace(".", "")?.ToLower()?.Trim() ?? throw new ArgumentNullException(nameof(extension));
            MIMEType = mimeType ?? throw new ArgumentNullException(nameof(mimeType));
            Signature = signature ?? new List<byte[]>();
        }

        /// <summary>
        /// File extension in lower format excluding the starting period (.).
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// Registered MIMEType for the given extension.
        /// </summary>
        public string MIMEType { get; }

        /// <summary>
        /// Byte signature for the given extension. Optional. Used for validation.
        /// </summary>
        public IEnumerable<byte[]> Signature { get; }
    }
}
