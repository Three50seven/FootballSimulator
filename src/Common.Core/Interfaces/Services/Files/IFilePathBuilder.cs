namespace Common.Core
{
    public interface IFilePathBuilder
    {
        string BuildFullPath(string relativePath);
    }
}