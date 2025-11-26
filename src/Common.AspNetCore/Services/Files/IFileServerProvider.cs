using System.Collections.Generic;

namespace Common.AspNetCore.Services
{
    /// <summary>
    /// Provider for handling multiple file servers based on the relative/virtual/request path.
    /// </summary>
    public interface IFileServerProvider
    {
        /// <summary>
        /// Gets the FileProviderContext with <see cref="FileProviderContext.FileProvider"/> and <see cref="FileProviderContext.File"/> 
        /// to access a physical location by using its virtual path.
        /// </summary>
        FileProviderContext GetProviderContext(string virtualPath);
    }
}
