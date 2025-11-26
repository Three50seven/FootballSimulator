using System.IO;

namespace Common.Core.Domain
{
    public class DocumentUpload : FileUpload
    {
        public DocumentUpload(
            Stream stream, 
            string fileName, 
            int directoryId, 
            long length = 0, 
            string contentType = null)
            : base (stream, fileName, length, contentType)
        {
            DirectoryId = directoryId;
        }

        public int DirectoryId { get; }
    }
}
