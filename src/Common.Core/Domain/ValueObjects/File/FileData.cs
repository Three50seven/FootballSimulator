using System.IO;

namespace Common.Core.Domain
{
    public class FileData : ValueObject<FileData>
    {
        protected FileData() { }

        public FileData(MemoryStream ms, string fileName)
           : this(ms?.ToArray(), fileName, FileExtensions.Instance.LookupMIMETypeFromFileName(fileName))
        {

        }

        public FileData(MemoryStream ms, string fileName, string contentType) 
            : this(ms?.ToArray(), fileName, contentType)
        {

        }

        public FileData(byte[] data, string fileName)
            : this (data, fileName, FileExtensions.Instance.LookupMIMETypeFromFileName(fileName))
        {

        }

        public FileData(byte[] data, string fileName, string contentType)
        {
            Data = data;
            FileName = fileName.SetEmptyToNull();
            ContentType = contentType.SetEmptyToNull();
        }

        public byte[] Data { get; private set; }
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
    }
}
