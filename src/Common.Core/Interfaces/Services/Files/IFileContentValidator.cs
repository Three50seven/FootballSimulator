namespace Common.Core.Interfaces
{
    public interface IFileContentValidator
    {
        string Extension { get; }
        void Validate(byte[] fileData);
    }
}
