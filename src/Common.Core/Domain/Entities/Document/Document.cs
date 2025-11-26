using Common.Core.DTOs;
using Common.Core.Validation;

namespace Common.Core.Domain
{
    public class Document : DomainEntity, ICopyable<Document>
    {
        protected Document() { }

        public Document(FileDetail file, DocumentDirectory directory) 
            : this(file, directory, null!)
        {
        }

        public Document(FileDetail file, DocumentDirectory directory, string subPath) 
            : this(file, directory, subPath, null!)
        {
        }

        public Document(FileDetail file, DocumentDirectory directory, string subPath, string name)
        {
            Guard.IsNotNull(file, nameof(file));
            Guard.IsNotNull(directory, nameof(directory));

            Name = string.IsNullOrWhiteSpace(name) ? file.FileName : name.Trim();
            SubPath = subPath.SetEmptyToNull();
            File = file;
            UpdateDirectory(directory);
            Directory = directory;
        }

        public string? Name { get; set; }
        public FileDetail? File { get; protected set; }
        public string? SubPath { get; set; }

        public int DirectoryId { get; protected set; }
        public DocumentDirectory? Directory { get; protected set; }

        public virtual string FullPath => $"{Directory?.Path.ToDirectoryPathFormat()}{SubPath?.ToDirectoryPathFormat()}{File?.FileName}";

        public virtual void UpdateDirectory(DocumentDirectory directory)
        {
            Guard.IsNotNull(directory, nameof(directory));
            Guard.IsNotNew(directory, nameof(directory));

            if (!directory.ExtensionAccepted(File?.Extension!))
                throw new ValidationException(DocumentDirectory.FileExtensionNotAllowedRule(File?.Extension!));

            Directory = directory;
        }

        public virtual DocumentSimpleItem ToSimpleItem()
        {
            return new DocumentSimpleItem(Id, Guid, FullPath, File?.FileName!, Name!);
        }

        public virtual DocumentInputItem ToInputItem()
        {
            return ToInputItem(DirectoryId);
        }

        public virtual DocumentInputItem ToInputItem(int directoryId)
        {
            return new DocumentInputItem(directoryId)
            {
                Id = Id,
                FullPath = FullPath,
                FileName = File?.FileName!,
                Name = Name!
            };
        }

        public override string ToString()
        {
            return FullPath;
        }

        public virtual Document Copy()
        {
            return new Document(new FileDetail(File?.FileName!, File?.ContentLength!, File?.ContentType!),
                                Directory!,
                                SubPath!,
                                Name!);
        }
    }
}
