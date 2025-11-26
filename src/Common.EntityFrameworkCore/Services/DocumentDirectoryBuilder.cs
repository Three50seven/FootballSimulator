using Humanizer;
using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Collections.Generic;

namespace Common.EntityFrameworkCore
{
    /// <summary>
    /// Established all values needed for a <see cref="DocumentDirectory"/> based on a custom enum value of type <typeparamref name="TEnumType"/>.
    /// There should be only one builder per enum value for this type <typeparamref name="TEnumType"/>.
    /// </summary>
    /// <typeparam name="TEnumType"></typeparam>
    public sealed class DocumentDirectoryBuilder<TEnumType> where TEnumType : Enum
    {
        /// <summary>
        /// Create builder for a directory based on <paramref name="enumValue"/>.
        /// </summary>
        /// <param name="enumValue">Custom enum value that represents a directory to build.</param>
        /// <param name="path">Optional path value. Defaults to the plural, underscored string version of <paramref name="enumValue"/>.</param>
        /// <param name="includeRoot">Include a previously defined "root" directory for all directories. Defaults to true.</param>
        /// <param name="maxFileSize">Optional maximum file size in bytes allowed to be uploaded to the directory. Defaults to no limit.</param>
        /// <param name="extensions">List of file extensions allowed in this directory. <see cref="FileExtensions.CommonDocumentExtensions"/> will be used by default.</param>
        public DocumentDirectoryBuilder(
            TEnumType enumValue, 
            string path = null, 
            bool includeRoot = true, 
            long maxFileSize = 0,
            IEnumerable<string> extensions = null)
        {
            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue));

            // format the enum value as the path/folder name if a custom path was not provided
            if (string.IsNullOrWhiteSpace(path))
                path = enumValue.ToString().Pluralize(inputIsKnownToBeSingular: true).Underscore();

            path = path.Trim().ToLower();

            if (!path.EndsWith('/'))
                path += "/";

            if (includeRoot && path.StartsWith('/'))
                path = path[1..];
            else if (!includeRoot && !path.StartsWith('/'))
                path = "/" + path;

            Path = path;
            IncludeRoot = includeRoot;
            MaxFileSize = maxFileSize;

            extensions ??= FileExtensions.CommonDocumentExtensions;

            foreach (var ext in extensions)
            {
                Extensions.Add(ext);
            }
        }

        internal string Path { get; private set; }
        internal HashSet<string> Extensions { get; private set; } = new HashSet<string>();
        internal bool IncludeRoot { get; private set; }
        internal long MaxFileSize { get; private set; }

        public DocumentDirectoryBuilder<TEnumType> AddExtension(string ext)
        {
            Guard.IsNotNull(ext, nameof(ext));
            Extensions.Add(ext);
            return this;
        }

        public DocumentDirectoryBuilder<TEnumType> AddExtensions(params string[] extensions)
        {
            if (extensions != null)
            {
                foreach (var ext in extensions)
                {
                    Extensions.Add(ext);
                }
            }
            return this;
        }
    }
}
