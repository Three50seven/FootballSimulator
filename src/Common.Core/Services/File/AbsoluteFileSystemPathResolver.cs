using Common.Core.Interfaces;
using System;
using System.IO;

namespace Common.Core.Services
{
    /// <summary>
    /// Resolver that resolves a relative path to a physical file starting from an absolute root path.
    /// Root is set via <see cref="FileStorageSettings.Root"/>.
    /// </summary>
    public class AbsoluteFileSystemPathResolver : IFileSystemPathResolver
    {
        private readonly FileStorageSettings _fileStorageSettings;

        public AbsoluteFileSystemPathResolver(
            FileStorageSettings fileStorageSettings)
        {
            _fileStorageSettings = fileStorageSettings;
        }

        public string ResolveFullPath(string relativePath, bool canBeDirectory = false)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;

            if (PathHelper.IsAbsolutePath(relativePath))
            {
                if (Directory.Exists(relativePath) && !canBeDirectory)
                    throw new InvalidOperationException($"Path '{relativePath}' is found to be a directory when path is required to be a file under the current operation.");

                return relativePath;
            }

            if (!string.IsNullOrWhiteSpace(_fileStorageSettings.RelativeRootDirectory))
                relativePath = relativePath.Replace(_fileStorageSettings.RelativeRootDirectory, "");

            string absolutePath = PathHelper.GetAbsolutePath(relativePath, _fileStorageSettings.Root);

            if (Directory.Exists(absolutePath) && !canBeDirectory)
                throw new InvalidOperationException($"Path '{relativePath}' is found to be a directory when path is required to be a file under the current operation.");

            return absolutePath;
        }
    }
}
