using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Common.Core.Validation;

namespace Common.AspNetCore.Services
{
    /// <summary>
    /// Provider for handling multiple file servers based on the relative/virtual/request path.
    /// Checks provided custom file servers based on request path. Falls back to checking content root and web root directories.
    /// </summary>
    public class FileServerProvider : IFileServerProvider
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileServerProvider> _logger;
        private readonly IEnumerable<WebFileServerOptions> _fileServerOptionsCollection;

        public FileServerProvider(
            WebFileServersConfigurationSettings fileServersConfigurationSettings,
            IWebHostEnvironment environment,
            ILogger<FileServerProvider> logger)
        {
            _fileServerOptionsCollection = fileServersConfigurationSettings.FileServerOptions;
            _environment = environment;
            _logger = logger;
        }

        public virtual FileProviderContext GetProviderContext(string virtualPath)
        {
            Guard.IsNotNull(virtualPath, nameof(virtualPath));

            // check registred file servers first based on the path provided
            if (_fileServerOptionsCollection != null)
            {
                var fileServer = _fileServerOptionsCollection.FirstOrDefault(s => s.Matches(virtualPath));

                // if found, use that file server to respond
                if (fileServer != null)
                {
                    return new FileProviderContext(
                            fileServer.FileProvider!,
                            virtualPath[fileServer.RequestPath.Value!.Length..], // specify filepath excluding the designated request path at the start
                            fileServer.PhysicalPath,
                            virtualPath);
                }
            }
            
            // no file servers found, fall back to checking webroot provider for the file
            if (_environment.WebRootFileProvider.TryGetDirectoryOrFile(virtualPath, _environment.WebRootPath, out FileProviderContext context))
                return context;

            // no file found in webroot, fall back to the content root
            if (_environment.ContentRootFileProvider.TryGetDirectoryOrFile(virtualPath, _environment.ContentRootPath, out context))
                return context;

            _logger.LogWarning($"Virtual path '{virtualPath}' does not resolve to a file on any registered server options, application content root, or wwwroot.");
            
            // no file found
            return null!;
        }
    }
}