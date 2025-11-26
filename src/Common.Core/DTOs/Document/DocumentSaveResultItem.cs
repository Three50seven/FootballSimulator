using Common.Core.Domain;
using Common.Core.Validation;

namespace Common.Core.DTOs
{
    public class DocumentSaveResultItem : DocumentSimpleItem
    {
        public DocumentSaveResultItem(Document document)
        {
            Guard.IsNotNull(document, nameof(document));

            Id = document.Id;
            Guid = document.Guid;
            FullPath = document.FullPath;
            FileName = document.File.FileName;
            Name = document.Name;
            IsImage = document.File.IsImage;
        }
    }
}
