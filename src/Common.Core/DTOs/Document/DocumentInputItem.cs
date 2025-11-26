namespace Common.Core.DTOs
{
    public class DocumentInputItem
    {
        public DocumentInputItem() { }

        public DocumentInputItem(int directoryId)
        {
            DirectoryId = directoryId;
        }
        
        public int? Id { get; set; }
        public int DirectoryId { get; set; }
        public string FullPath { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }

        public bool IsSet => Id.CleanForNull() != null;

        public void SetDocInfo(DocumentSimpleItem docInfo)
        {
            if (docInfo == null)
            {
                Id = null;
                FileName = string.Empty;
                FullPath = string.Empty;
                Name = string.Empty;
                return;
            }

            Id = docInfo.Id;
            FullPath = docInfo.FullPath;
            FileName = docInfo.FileName;
            Name = docInfo.Name;
        }

        public DocumentSimpleItem ToSimpleDocInfo()
        {
            return !IsSet ? null : new DocumentSimpleItem((int)Id, FullPath, FileName, Name);
        }
    }
}
