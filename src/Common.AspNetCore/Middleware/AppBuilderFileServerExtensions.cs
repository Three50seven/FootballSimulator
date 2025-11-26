using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.AspNetCore.Services;
using Common.Core.Validation;
using System.Collections.Generic;

namespace Common.AspNetCore
{
    public static class AppBuilderFileServerExtensions
    {
        /// <summary>
        /// Wrapper for <see cref="UseWebFileServer(IApplicationBuilder, WebFileServerOptions)"/> 
        /// to easily use the <see cref="WebFileServerOptions"/> registered in the <see cref="IFileServerProvider"/>.
        /// Call <see cref="WebFileServerServiceCollectionExtensions.AddWebServerFileStorage(IServiceCollection, IConfiguration, string, Core.FileStorageSettings)"/> 
        /// or <see cref="WebFileServerServiceCollectionExtensions.AddWebServerFileStorage(IServiceCollection, WebFileServersConfigurationSettings, Core.FileStorageSettings)"/> prior to
        /// initializing this in the pipeline.
        /// </summary>
        /// <param name="app">Established application builder on the executing application.</param>
        /// <param name="configurationSettings">Optional configuration settings. If not supplied, settings will be pulled from service registration.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebFileServers(
            this IApplicationBuilder app,
            WebFileServersConfigurationSettings configurationSettings = null)
        {
            configurationSettings ??= app.ApplicationServices.GetRequiredService<WebFileServersConfigurationSettings>();

            return UseWebFileServers(app, configurationSettings.FileServerOptions);
        }

        /// <summary>
        /// Wrapper for <see cref="UseWebFileServer(IApplicationBuilder, WebFileServerOptions)"/> 
        /// to easily use the <see cref="WebFileServerOptions"/> passed through <paramref name="fileServerOptions"/>.
        /// </summary>
        /// <param name="app">Established application builder on the executing application.</param>
        /// <param name="fileServerOptions">Required options for one or more file servers.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebFileServers(
            this IApplicationBuilder app,
            IEnumerable<WebFileServerOptions> fileServerOptions)
        {
            Guard.IsNotNull(fileServerOptions, nameof(fileServerOptions));

            foreach (var option in fileServerOptions)
            {
                UseWebFileServer(app, option);
            }

            return app;
        }

        /// <summary>
        /// Custom file server setup for <see cref="WebFileServerOptions"/> <paramref name="options"/>.
        /// Mimics same setup as Microsoft's <see cref="FileServerExtensions.UseFileServer(IApplicationBuilder, FileServerOptions)"/>.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options">Custom web file server options.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebFileServer(this IApplicationBuilder app, WebFileServerOptions options)
        {
            Guard.IsNotNull(app, nameof(app));
            Guard.IsNotNull(options, nameof(options));

            if (options.EnableDefaultFiles)
                app.UseDefaultFiles(options.DefaultFilesOptions);
            
            if (options.EnableDirectoryBrowsing)
                app.UseDirectoryBrowser(options.DirectoryBrowserOptions);

            return app.UseStaticFiles(options.StaticFileOptions);
        }
    }
}
