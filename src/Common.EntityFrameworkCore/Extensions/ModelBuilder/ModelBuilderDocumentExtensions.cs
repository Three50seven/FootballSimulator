using Microsoft.EntityFrameworkCore;
using Common.Core;
using Common.Core.Validation;
using System;
using System.Collections.Generic;

namespace Common.EntityFrameworkCore
{
    public static class ModelBuilderDocumentExtensions
    {
        /// <summary>
        /// Applies entity configurations for standard <see cref="Common.Core.Domain.Document"/> and relations.
        /// Optionally provide path lookup dictionary to die custom directory enums with associated directory path.
        /// </summary>
        /// <typeparam name="TDirectoryEnum">Custom enum type for document directories.</typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="root">Document root directory. Typically something like '/site_content' or '/app_content'.</param>
        /// <param name="fileExtensions">Optional set of file extensions with MIME types that should be included. Defaulted to all currently known types.</param>
        /// <param name="directoryBuilderLookup">Optional lookup for enum directory paths and allowed extensions. By default, enum name will be use; pluraled, lowered, and snake_cased with common document extensions allowed.</param>
        /// <returns></returns>
        public static ModelBuilder AddDocumentConfigurations<TDirectoryEnum>(
            this ModelBuilder modelBuilder, 
            string root = "/site_content",
            FileExtensions fileExtensions = null,
            IDictionary<TDirectoryEnum, DocumentDirectoryBuilder<TDirectoryEnum>> directoryBuilderLookup = null)
            where TDirectoryEnum : Enum
        {
            Guard.IsNotNull(modelBuilder, nameof(modelBuilder));

            if (fileExtensions == null)
                fileExtensions = new FileExtensions();
            else
                FileExtensions.Instance = fileExtensions;

            var documentExtensions = fileExtensions.ToDocumentExtensions();

            directoryBuilderLookup ??= new Dictionary<TDirectoryEnum, DocumentDirectoryBuilder<TDirectoryEnum>>();

            var enumValues = (TDirectoryEnum[])Enum.GetValues(typeof(TDirectoryEnum));
            if (enumValues != null && enumValues.Length > 0)
            {
                // add any missing enum values with all defaults
                foreach (var enumValue in enumValues)
                {
                    if (!directoryBuilderLookup.ContainsKey(enumValue))
                        directoryBuilderLookup.Add(enumValue, new DocumentDirectoryBuilder<TDirectoryEnum>(enumValue));
                }
            }

            return modelBuilder.ApplyConfiguration(new DocumentExtensionConfiguration(documentExtensions.Values))
                               .ApplyConfiguration(new DocumentConfiguration())
                               .ApplyConfiguration(new DocumentDirectoryConfiguration<TDirectoryEnum>(directoryBuilderLookup, root))
                               .ApplyConfiguration(new DocumentDirectoryExtensionConfiguration<TDirectoryEnum>(documentExtensions, directoryBuilderLookup));
        }
    }
}
