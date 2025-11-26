using System.IO;

namespace Common.Core.Domain
{
    public class FileUpload : ValueObject<FileUpload>
    {
        public FileUpload(Stream stream, string fileName, long length = 0, string contentType = null)
        {
            Stream = stream;
            FileName = fileName;
            Length = length < 0 ? 0 : length;

            if (string.IsNullOrWhiteSpace(contentType))
                contentType = FileExtensions.Instance.LookupMIMETypeFromFileName(fileName);

            ContentType = contentType.ToLower();
        }

        public Stream Stream { get; }
        public string FileName { get; }
        public long Length { get; }
        public string ContentType { get; }

        public DocumentUpload ToDocumentUpload(int directoryId)
        {
            return new DocumentUpload(Stream, FileName, directoryId, Length, ContentType);
        }
    }
}
