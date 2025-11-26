using Common.Core.Services;
using Common.Core.Validation;
using System;
using System.Collections.Generic;
using System.IO;

namespace Common.Core.Domain
{
    public class FileDirectory : ValueObject<FileDirectory>, IFileSystemItem
    {
        public FileDirectory(string virtualPath, DirectoryInfo directory)
        {
            Guard.IsNotNull(directory, nameof(directory));

            Name = directory.Name;
            PhysicalPath = directory.FullName;
            VirtualPath = virtualPath ?? throw new ArgumentNullException(nameof(virtualPath));
            CreateDate = directory.CreationTimeUtc;
            LastModifiedDate = directory.LastWriteTimeUtc;
        }
        public FileDirectory(
            string name, 
            string physicalPath, 
            string virtualPath, 
            DateTime createDate, 
            DateTime lastModifiedDate) 
        {
            Name = name;
            PhysicalPath = physicalPath;
            VirtualPath = virtualPath;
            CreateDate = createDate;
            LastModifiedDate = lastModifiedDate;
        }

        public string Name { get; private set; }
        public string PhysicalPath { get; private set; }
        public string VirtualPath { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime LastModifiedDate { get; private set; }

        public IEnumerable<FileDirectory> Directories { get; private set; } = new List<FileDirectory>();
        public IEnumerable<FilePathInfo> Files { get; private set; } = new List<FilePathInfo>();

        public FileDirectory AddSubDirectory(FileDirectory directory)
        {
            Guard.IsNotNull(directory, nameof(directory));

            (Directories as List<FileDirectory>).Add(directory);
            return directory;
        }

        public FilePathInfo AddFile(FilePathInfo file)
        {
            Guard.IsNotNull(file, nameof(file));

            (Files as List<FilePathInfo>).Add(file);
            return file;
        }

        public void LoadLocalChildren(
           FileDirectoryIncludeOption directoryIncludes,
           FilesIncludeOption fileIncludes,
           int level)
        {
            (Directories as List<FileDirectory>).Clear();

            switch (fileIncludes)
            {
                case FilesIncludeOption.None:
                    break;
                case FilesIncludeOption.LevelOne:
                    if (level == 1)
                        LoadLocalChildrenFiles();
                    break;
                case FilesIncludeOption.AllRecursive:
                    LoadLocalChildrenFiles();
                    break;
                default:
                    break;
            }

            switch (directoryIncludes)
            {
                case FileDirectoryIncludeOption.None:
                    return;
                case FileDirectoryIncludeOption.LevelOneSubDirectories:
                    if (level > 1)
                        return;
                    break;
                case FileDirectoryIncludeOption.AllRecursive:
                    break;
                default:
                    break;
            }

            var directoryPaths = Directory.GetDirectories(PhysicalPath);
            if (directoryPaths != null)
            {
                foreach (var directoryPath in directoryPaths)
                {
                    var subDirectoryInfo = new DirectoryInfo(directoryPath);
                    var subDirectory = new FileDirectory(PathHelper.Combine(VirtualPath, subDirectoryInfo.Name), subDirectoryInfo);
                    subDirectory.LoadLocalChildren(directoryIncludes, fileIncludes, level + 1);
                    AddSubDirectory(subDirectory);
                }
            }
        }

        public void LoadLocalChildrenFiles()
        {
            (Files as List<FilePathInfo>).Clear();

            var filePaths = Directory.GetFiles(PhysicalPath);
            if (filePaths != null)
            {
                foreach (var filePath in filePaths)
                {
                    var file = new FileInfo(filePath);
                    AddFile(new FilePathInfo(PathHelper.Combine(VirtualPath, file.Name), file));
                }
            }
        }
    }
}
