using Common.Core.Interfaces;
using System;
using System.IO;

namespace Common.Core.Services
{
    /// <summary>
    /// Default implementation on resolving file paths.
    /// Relative path is only validated against local directories and returned.
    /// </summary>
    public class RelativeFileSystemPathResolver : IFileSystemPathResolver
    {
        public string ResolveFullPath(string relativePath, bool canBeDirectory = false)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;

            if (Directory.Exists(relativePath) && !canBeDirectory)
                throw new InvalidOperationException($"Path '{relativePath}' is found to be a directory when path is required to be a file under the current operation.");

            if (!PathHelper.IsAbsolutePath(relativePath))
                relativePath = relativePath.SetNullToEmpty().Replace("/", @"\");

            return relativePath;
        }
    }
}
