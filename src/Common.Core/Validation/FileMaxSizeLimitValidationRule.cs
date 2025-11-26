namespace Common.Core.Validation
{
    public class FileMaxSizeLimitValidationRule : DetailedValidationRule
    {
        public FileMaxSizeLimitValidationRule(long fileSize, long maxSize) 
            : base($"File exceeds maximum size limit.", $"File size: {fileSize}. Max file size: {maxSize}.")
        {
        }
    }
}
