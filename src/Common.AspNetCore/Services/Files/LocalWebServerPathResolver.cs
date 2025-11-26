using Common.Core.Interfaces;
using Common.Core.Services;
using System;
using System.IO;

namespace Common.AspNetCore.Services
{
    /// <summary>
    /// Resolves relative paths by using <see cref="IFileServerProvider"/> 
    /// to resolve from custom file servers, webroot, and content root on disk.
    /// </summary>
    public class LocalWebServerPathResolver : IFileSystemPathResolver
    {
        private readonly IFileServerProvider _fileServerProvider;

        public LocalWebServerPathResolver(
           IFileServerProvider fileServerProvider)
        {
            _fileServerProvider = fileServerProvider;
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

            relativePath = relativePath.Replace("\\", "/");
            relativePath = relativePath.StartsWith('/') ? relativePath : string.Concat("/", relativePath);

            var context = _fileServerProvider.GetRequiredProviderContext(relativePath);
            if (context == null)
                return null;

            // file may or may not exist but if the physical path exists, the path is considered a valid file path
            if (context.File.PhysicalPath != null)
                return context.File.PhysicalPath;

            // otherwise, path may be a directory - try to return the physical path if found
            var directory = context.FileProvider.GetDirectoryContents("");
            if (directory.Exists)
            {
                if (!canBeDirectory)
                    throw new InvalidOperationException($"Path '{context.PhysicalPath}' is found to be a directory when path is required to be a file under the current operation.");

                return context.PhysicalPath;
            }

            return null;
        }
    }
}
