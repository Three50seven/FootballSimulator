using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using System;

namespace Common.AspNetCore
{
    /// <summary>
    /// Custom file server options based on Microsoft's <see cref="FileServerOptions"/>.
    /// This version builds upon what Microsoft has, but allows for controlling available properties
    /// via public getters and setters. Also requires instance of <see cref="Microsoft.AspNetCore.Builder.StaticFileOptions"/>.
    /// Use custom <see cref="StaticFileCacheDefaultOptions"/> to set caching options on static files.
    /// </summary>
    public class WebFileServerOptions : SharedOptionsBase
    {
        public WebFileServerOptions(StaticFileOptions staticFileOptions)
           : this (staticFileOptions, new SharedOptions())
        {

        }

        public WebFileServerOptions(StaticFileOptions staticFileOptions, SharedOptions sharedOptions)
           : base(sharedOptions)
        {
            StaticFileOptions = staticFileOptions;
            DirectoryBrowserOptions = new DirectoryBrowserOptions(SharedOptions);
            DefaultFilesOptions = new DefaultFilesOptions(SharedOptions);
            EnableDefaultFiles = true;
        }

        /// <summary>
        /// Options for configuring the StaticFileMiddleware.
        /// </summary>
        public StaticFileOptions StaticFileOptions { get; set; }

        /// <summary>
        /// Options for configuring the DirectoryBrowserMiddleware.
        /// </summary>
        public DirectoryBrowserOptions DirectoryBrowserOptions { get; set; }

        /// <summary>
        /// Options for configuring the DefaultFilesMiddleware.
        /// </summary>
        public DefaultFilesOptions DefaultFilesOptions { get; set; }

        /// <summary>
        /// Directory browsing is disabled by default.
        /// </summary>
        public bool EnableDirectoryBrowsing { get; set; }

        /// <summary>
        /// Default files are enabled by default.
        /// </summary>
        public bool EnableDefaultFiles { get; set; }

        /// <summary>
        /// Physical absolute path to the web file server directory.
        /// </summary>
        public string PhysicalPath { get; set; }

        /// <summary>
        /// Check a given virtual path to see if the path matches with this web file server.
        /// The virtual path is checked to start with the <see cref="SharedOptionsBase.RequestPath"/> ignoring case.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public bool Matches(string virtualPath)
        {
            return virtualPath?.StartsWith(StaticFileOptions.RequestPath, StringComparison.InvariantCultureIgnoreCase) ?? false;
        }
    }
}
