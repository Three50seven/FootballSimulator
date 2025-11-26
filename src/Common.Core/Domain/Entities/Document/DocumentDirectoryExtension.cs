namespace Common.Core.Domain
{
    public class DocumentDirectoryExtension : IEntity
    {
        protected DocumentDirectoryExtension() { }

        public DocumentDirectoryExtension(int directoryId, int extensionId)
        {
            DirectoryId = directoryId;
            ExtensionId = extensionId;
        }

        public DocumentDirectoryExtension(DocumentDirectory directory, DocumentExtension extension)
        {
            Directory = directory;
            Extension = extension;
        }

        public int DirectoryId { get; private set; }
        public virtual DocumentDirectory? Directory { get; private set; }

        public int ExtensionId { get; private set; }
        public virtual DocumentExtension? Extension { get; private set; }
    }
}
