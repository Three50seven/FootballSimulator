using Common.Core;
using Common.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    public static class FileExtensionsExtensions
    {
        /// <summary>
        /// Get list of lookup entity DocumentExtension types.
        /// NOTE: The Id provided will based from Default Extensions ordered by key and then any additional custom keys provided.
        /// Only call this method once during application setup and do not alter custom keys or the default list after application has been initialized.
        /// </summary>
        /// <returns>List of DocumentExtensions. Typically used to include in seeding application database.</returns>
        internal static IDictionary<string, DocumentExtension> ToDocumentExtensions(this FileExtensions fileExtensions)
        {
            var docExtensions = new Dictionary<string, DocumentExtension>();
            var keys = fileExtensions.Keys.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                docExtensions.Add(keys[i], new DocumentExtension(i + 1, keys[i], fileExtensions[keys[i]].MIMEType));
            }

            return docExtensions;
        }
    }
}
