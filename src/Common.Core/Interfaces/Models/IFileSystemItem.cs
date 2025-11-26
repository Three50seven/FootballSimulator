using System;

namespace Common.Core
{
    public interface IFileSystemItem
    {
        string Name { get; }
        string PhysicalPath { get; }
        string VirtualPath { get; }
        DateTime CreateDate { get; }
        DateTime LastModifiedDate { get; }
    }
}
