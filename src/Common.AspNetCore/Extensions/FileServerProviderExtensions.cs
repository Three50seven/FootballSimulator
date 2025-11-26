using Common.AspNetCore.Services;
using Common.Core;
using System;

namespace Common.AspNetCore
{
    public static class FileServerProviderExtensions
    {
        /// <summary>
        /// Gets the FileProviderContext with <see cref="FileProviderContext.FileProvider"/> and <see cref="FileProviderContext.File"/> 
        /// to access a physical location by using its virtual path.
        /// Throws exception if context for the given path is not found.
        /// </summary>
        /// <param name="fileServerProvider"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static FileProviderContext GetRequiredProviderContext(this IFileServerProvider fileServerProvider, string path)
        {
            var providerContext = fileServerProvider.GetProviderContext(path) ?? throw new InvalidOperationException($"FileProvider for path '{path}' was not found.".Sanitize());
            return providerContext;
        }
    }
}
