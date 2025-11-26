using Microsoft.Extensions.FileProviders;
using Common.Core;
using Common.Core.Domain;
using System;

namespace Common.AspNetCore
{
    /// <summary>
    /// Represents the result when looking up established file providers (essentially file "servers") based on a virtual path.
    /// </summary>
    public class FileProviderContext : ValueObject<FileProviderContext>
    {
        public FileProviderContext(IFileProvider fileProvider, string requestPath)
            : this(fileProvider, requestPath, requestPath, requestPath)
        {

        }

        public FileProviderContext(IFileProvider fileProvider, string filePath, string physicalPath, string requestPath)
        {
            FileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            RequestPath = requestPath.SetNullToEmpty();
            File = fileProvider.GetFileInfo(filePath);
            PhysicalPath = physicalPath;
        }

        public IFileProvider FileProvider { get; }
        public string RequestPath { get; }
        public string PhysicalPath { get; }
        public IFileInfo File { get; }
    }
}
