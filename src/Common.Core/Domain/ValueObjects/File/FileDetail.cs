using Common.Core.Validation;

namespace Common.Core.Domain
{
    public class FileDetail : ValueObject<FileDetail>
    {
        protected FileDetail() { }

        public FileDetail(string fileName, long? contentLength = 0, string? contentType = null)
        {
            Guard.IsNotNull(fileName, nameof(fileName));

            if (string.IsNullOrWhiteSpace(contentType))
                contentType = FileExtensions.Instance.LookupMIMETypeFromFileName(fileName);

            Extension = Path.GetExtension(fileName).Replace(".", "").ToLower();
            FileName = fileName;
            ContentType = contentType.Trim().ToLower();
            ContentLength = contentLength < 0 ? 0 : contentLength;
        }

        public string? FileName { get; private set; }
        public string? ContentType { get; private set; }
        public long? ContentLength { get; private set; }
        public string? Extension { get; private set; }

        public virtual bool IsImage => ImageExtensions.Any(e => string.Compare(e, Extension, StringComparison.CurrentCultureIgnoreCase) == 0);
        public static string[] ImageExtensions = new string[] { "jpg", "jpeg", "png", "gif" };
    }
}
