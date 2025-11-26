using Common.Core.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Common.Core
{
    public sealed partial class FileExtensions : ReadOnlyDictionary<string, FileExtension>
    {
        /// <summary>
        /// Create dictionary of known and custom file extensions and MIME types designed for single use in an application.
        /// </summary>
        /// <param name="includeDefaults">Include default/known file extension and MIME types in list. Defaults to true.</param>
        /// <param name="extensions">Custom list of extension and MIME types. NOTE: Do not alter or reorder this list after application has been initialized.</param>
        public FileExtensions(
            bool includeDefaults = true, 
            IDictionary<string, FileExtension> extensions = null)
            : base (InitializeDictionary(includeDefaults, extensions))
        {

        }

        private static IDictionary<string, FileExtension> InitializeDictionary(bool includeDefaults, IDictionary<string, FileExtension> extensions)
        {
            // initialize a dictionary - optionally with the default extensions
            var dictionary = includeDefaults ? new Dictionary<string, FileExtension>((Dictionary<string, FileExtension>)DefaultExtensions) 
                : new Dictionary<string, FileExtension>();

            // merge any custom extensions
            extensions?.ToList().ForEach(d => dictionary[d.Key] = d.Value);
            
            return dictionary;
        }

        /// <summary>
        /// Lookup MIME type based on filename. Uses filename's extension against default list of known MIME types.
        /// </summary>
        /// <param name="fileName">Valid filename with extension</param>
        /// <returns>Known MIME type for the file.</returns>
        public string LookupMIMETypeFromFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return "application/octet-stream";

            string extension = GetExtension(fileName);

            if (!string.IsNullOrWhiteSpace(extension) && ContainsKey(extension))
                return this[extension].MIMEType;

            return "application/octet-stream";
        }

        private static string GetExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            if (string.IsNullOrWhiteSpace(extension))
                return string.Empty;

            return extension.ToLowerInvariant().Remove(0, 1);
        }
    }
}
