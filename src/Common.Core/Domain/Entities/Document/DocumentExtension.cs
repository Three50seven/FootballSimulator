using System.Collections.Generic;
using System.Linq;

namespace Common.Core.Domain
{
    public class DocumentExtension : Entity<int>
    {
        protected DocumentExtension() { }

        public DocumentExtension(int id, string extension, string mimeType) 
            : base (id)
        {
            Extension = extension.SetEmptyToNull();
            MimeType = mimeType.SetEmptyToNull();
        }

        public string? Extension { get; private set; }
        public string? MimeType { get; private set; }

        public IEnumerable<DocumentDirectoryExtension> DocumentDirectoryExtensions { get; set; } = new List<DocumentDirectoryExtension>();
        public IEnumerable<DocumentDirectory> Directories => DocumentDirectoryExtensions.Select(de => de.Directory!);
    }
}
