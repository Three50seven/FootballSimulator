using Common.Core.Validation;
using System;
using System.IO;

namespace Common.Core.Domain
{
    public class FilePathInfo : ValueObject<FilePathInfo>, IFileSystemItem
    {
        protected FilePathInfo() { }

        public FilePathInfo(string virtualPath, FileInfo file)
        {
            Guard.IsNotNull(file, nameof(file));

            Name = file.Name;
            DirectoryName = Path.GetDirectoryName(file.FullName);
            PhysicalPath = file.FullName;
            VirtualPath = virtualPath ?? throw new ArgumentNullException(nameof(virtualPath));
            CreateDate = file.CreationTimeUtc;
            LastModifiedDate = file.LastWriteTimeUtc;
        }

        public FilePathInfo(
            string name, 
            string directoryName, 
            string physicalPath, 
            string virtualPath,
            DateTime createDate,
            DateTime lastModifiedDate)
        {
            Name = name;
            DirectoryName = directoryName;
            PhysicalPath = physicalPath;
            VirtualPath = virtualPath;
            CreateDate = createDate;
            LastModifiedDate = lastModifiedDate;
        }

        public string Name { get; private set; }
        public string DirectoryName { get; private set; }
        public string PhysicalPath { get; private set; }
        public string VirtualPath { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime LastModifiedDate { get; private set; }
    }
}
