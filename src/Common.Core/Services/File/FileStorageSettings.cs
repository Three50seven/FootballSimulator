using Common.Core.Domain;
using Common.Core.Validation;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core
{
    public class FileStorageSettings
    {
        /// <summary>
        /// Set to false if any missing directories found in a path should be automatically created. Default is true.
        /// </summary>
        public bool CreateDirectories { get; set; } = true;

        /// <summary>
        /// Set to true to prevent files from being overwritten. Default is false.
        /// </summary>
        public bool DenyOverwrites { get; set; } = false;

        /// <summary>
        /// Optional root directory path for file storage. Defaults to empty.
        /// Used in <see cref="Common.Core.Services.AbsoluteFileSystemPathResolver"/>.
        /// </summary>
        public string Root { get; set; } = string.Empty;

        /// <summary>
        /// Optional directory path that represents the relative path for finding files.
        /// Typically this is a root <see cref="DocumentDirectory.Path"/> value like "/site_content".
        /// Used in <see cref="Common.Core.Services.AbsoluteFileSystemPathResolver"/> to be removed from the relative path when resolving the absolute path.
        /// </summary>
        public string RelativeRootDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Global value for max file size/length in bytes allowed for uploads/storage.
        /// Defaults to 10 MB (10485760)
        /// </summary>
        public long MaxFileSize { get; set; } = 10485760;

        /// <summary>
        /// Max character length allowed for file names. Defaults to 100.
        /// Ref - https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file#win32-file-namespaces
        /// </summary>
        public int MaxFileNameLength { get; set; } = 100;

        /// <summary>
        /// Max character length allowed for full path of directories and/or file. Defaults to 250.
        /// Ref - https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file#win32-file-namespaces
        /// </summary>
        public int MaxFullPathLength { get; set; } = 250;

        /// <summary>
        /// Number of random characters to append to filenames when making them unique.
        /// Used by <see cref="IFileValidator.SanitizeFileName(string, bool)"/>.
        /// Must have a value between 4 and 24.
        /// </summary>
        public int NumUniqueCharsToAdd { get; set; } = 8;

        /// <summary>
        /// Buffer size that should be utilized when creating a <see cref="System.IO.FileStream"/>.
        /// Default value is 4096.
        /// </summary>
        public int StreamBufferSize { get; set; } = 4096;

        /// <summary>
        /// List of whitelisted extensions allowed when storing files. Dictionary key is extension lowered and excluding starting period (.).
        /// Defaults to <see cref="FileExtensions.CommonFileExtensions"/>.
        /// Call <see cref="AddOrUpdateExtension(FileExtension)"/> to add or update extension information.
        /// Call <see cref="ClearExtensions"/> to clear out the whitelist.
        /// </summary>
        public IReadOnlyDictionary<string, FileExtension> AllowedExtensions { get; } = new ConcurrentDictionary<string, FileExtension>(
                                                                                               FileExtensions.Instance.Where(ext => FileExtensions.CommonFileExtensions.Contains(ext.Key))
                                                                                                                      .Select(ext => ext.Value)
                                                                                                                      .ToDictionary(ext => ext.Extension));

        /// <summary>
        /// Clear whitelist of allowed file extensions <see cref="AllowedExtensions"/>.
        /// </summary>
        /// <returns></returns>
        public FileStorageSettings ClearExtensions()
        {
            (AllowedExtensions as ConcurrentDictionary<string, FileExtension>).Clear();
            return this;
        }
        
        /// <summary>
        /// Adds or updates file extension to whitelist of <see cref="AllowedExtensions"/>.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public FileStorageSettings AddOrUpdateExtension(FileExtension extension)
        {
            Guard.IsNotNull(extension, nameof(extension));

            (AllowedExtensions as ConcurrentDictionary<string, FileExtension>).AddOrUpdate(extension.Extension, extension, (key, ext) => extension);
            return this;
        }

        /// <summary>
        /// Adds or updates file extensions to whitelist of <see cref="AllowedExtensions"/>.
        /// </summary>
        /// <param name="extensions"></param>
        /// <returns></returns>
        public FileStorageSettings AddOrUpdateExtensions(IEnumerable<FileExtension> extensions)
        {
            Guard.IsNotNullOrEmptyList(extensions, nameof(extensions));

            foreach (var ext in extensions)
            {
                AddOrUpdateExtension(ext);
            }

            return this;
        }
    }
}
