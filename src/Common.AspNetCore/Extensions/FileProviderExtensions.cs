using Microsoft.Extensions.FileProviders;

namespace Common.AspNetCore
{
    public static class FileProviderExtensions
    {
        /// <summary>
        /// Attempts to see if virtualPath resolves to file or directory on the provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="virtualPath">Relative path to file or directory.</param>
        /// <param name="rootPath">Directory root that represents the root directory of the provider.</param>
        /// <param name="context">Output context containing information regarding the directory or file.</param>
        /// <returns></returns>
        public static bool TryGetDirectoryOrFile(
            this IFileProvider provider, 
            string virtualPath, 
            string rootPath, 
            out FileProviderContext context)
        {
            context = null;

            if (provider == null || string.IsNullOrWhiteSpace(virtualPath))
                return false;

            if (string.IsNullOrWhiteSpace(rootPath))
                rootPath = "/";

            var file = provider.GetFileInfo(virtualPath);
            if (file != null && file.Exists)
            {
                context = new FileProviderContext(provider, virtualPath, file.PhysicalPath, virtualPath);
                return true;
            }

            var directory = provider.GetDirectoryContents(virtualPath);
            if (directory != null && directory.Exists)
            {
                context = new FileProviderContext(provider, virtualPath, rootPath, virtualPath);
                return true;
            }

            return false;
        }
    }
}
