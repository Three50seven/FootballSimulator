using Common.Core.Validation;
using System.Net;

namespace Common.Core.Domain
{
    public class DocumentDirectory : LookupEntity
    {
        protected DocumentDirectory() 
        {
            Path = null!;
        }

        public DocumentDirectory(Enum directoryEnum, string path, long maxFileSize = 0) 
            : this(directoryEnum.ToInt(), directoryEnum.AsFriendlyName(), path, maxFileSize)
        {

        }

        public DocumentDirectory(int id, string name, string path, long maxFileSize = 0) 
            : base(id, name)
        {
            Guard.IsNotNull(path, nameof(path));

            Path = path.Trim();
            MaxFileSize = maxFileSize < 0 ? 0 : maxFileSize;
        }

        public string Path { get; protected set; }
        public long MaxFileSize { get; protected set; }

        public virtual IEnumerable<DocumentDirectoryExtension> DocumentDirectoryExtensions { get; protected set; } = new List<DocumentDirectoryExtension>();
        public virtual IEnumerable<DocumentExtension> Extensions => DocumentDirectoryExtensions.Select(d => d.Extension!);

        public virtual void AddExtension(DocumentExtension extension)
        {
            if (!HasAllowedExtension(extension))
                ((List<DocumentDirectoryExtension>)DocumentDirectoryExtensions).Add(new DocumentDirectoryExtension(this, extension));
        }

        protected virtual bool HasAllowedExtension(DocumentExtension extension) => Extensions.Any(x => x.Id == extension.Id);

        public virtual bool ExtensionAccepted(string fileExtension)
        {
            fileExtension = fileExtension.SetNullToEmpty(true).Replace(".", "");
            if (string.IsNullOrWhiteSpace(fileExtension))
                return false;

            return Extensions.Any(x => string.Compare(fileExtension, x.Extension!.Replace(".", ""), StringComparison.OrdinalIgnoreCase) == 0);
        }

        public static ValidationRule FileExtensionNotAllowedRule(string ext)
        {
            return new ValidationRule($"The file extension '{WebUtility.HtmlEncode(ext)}' is not allowed in this directory.");
        }
    }
}
