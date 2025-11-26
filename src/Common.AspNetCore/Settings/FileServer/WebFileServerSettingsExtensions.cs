using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Common.Core.Services;
using Common.Core.Validation;
using System.IO;

namespace Common.AspNetCore
{
    public static class WebFileServerSettingsExtensions
    {
        public static WebFileServerOptions ToWebFileServerOptions(this WebFileServerSettings serverSettings)
        {
            Guard.IsNotNull(serverSettings, nameof(serverSettings));

            // adjust physical path if the path in the settings is relative (local development, typically)
            string physicalPath = serverSettings.PhysicalPath;
            if (serverSettings.PhysicalPathIsRelative)
                physicalPath = PathHelper.Combine(Directory.GetCurrentDirectory(), serverSettings.PhysicalPath.TrimStart('\\').TrimStart('/'));

            if (serverSettings.CreateDirectory)
                Directory.CreateDirectory(physicalPath);

            var sharedOptions = new SharedOptions()
            {
                RequestPath = serverSettings.RequestPath,
                FileProvider = new PhysicalFileProvider(physicalPath),
            };

            var staticFileOptions = new StaticFileCacheDefaultOptions(
                serverSettings.Cache?.Enabled ?? false,
                serverSettings.Cache?.Duration ?? StaticFilesExtensions.DefaultCacheControlAge,
                sharedOptions)
            {
                ServeUnknownFileTypes = serverSettings.AllowUnknownFileTypes
            };

            // load in any custom file types from settings
            if (serverSettings.ContentTypes != null && serverSettings.ContentTypes.Count > 0)
            {
                var contentTypeProvider = new FileExtensionContentTypeProvider();

                foreach (var contentTypeMap in serverSettings.ContentTypes)
                {
                    string extension = contentTypeMap.Key?.Trim();
                    if (!extension.StartsWith('.'))
                        extension = $".{extension}";

                    contentTypeProvider.Mappings[extension] = contentTypeMap.Value?.Trim();
                }

                staticFileOptions.ContentTypeProvider = contentTypeProvider;
            }

            return new WebFileServerOptions(staticFileOptions, sharedOptions)
            {
                EnableDirectoryBrowsing = serverSettings.AllowDirectoryBrowsing,
                PhysicalPath = physicalPath
            };
        }
    }
}
