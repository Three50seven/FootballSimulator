namespace Common.Core.Interfaces
{
    /// <summary>
    /// Resolve relative paths to files.
    /// </summary>
    public interface IFileSystemPathResolver
    {
        /// <summary>
        /// Resolve a relative path to a full path for that file or directory.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="canBeDirectory"></param>
        /// <returns></returns>
        string ResolveFullPath(string relativePath, bool canBeDirectory = false);
    }
}
