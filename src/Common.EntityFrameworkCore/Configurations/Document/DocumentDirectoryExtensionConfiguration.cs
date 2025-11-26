using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Collections.Generic;

namespace Common.EntityFrameworkCore
{
    public class DocumentDirectoryExtensionConfiguration<TDirectoryEnumType> : IEntityTypeConfiguration<DocumentDirectoryExtension>
        where TDirectoryEnumType : Enum
    {
        private readonly IDictionary<TDirectoryEnumType, DocumentDirectoryBuilder<TDirectoryEnumType>> _directoryBuilderLookup;
        private readonly IDictionary<string, DocumentExtension> _documentExtensions;

        public DocumentDirectoryExtensionConfiguration(
            IDictionary<string, DocumentExtension> documentExtensions,
            IDictionary<TDirectoryEnumType, DocumentDirectoryBuilder<TDirectoryEnumType>> directoryBuilderLookup)
        {
            Guard.IsNotNull(directoryBuilderLookup, nameof(directoryBuilderLookup));

            _documentExtensions = documentExtensions;
            _directoryBuilderLookup = directoryBuilderLookup;
        }

        public virtual void Configure(EntityTypeBuilder<DocumentDirectoryExtension> builder)
        {
            builder.HasKey(t => new { t.DirectoryId, t.ExtensionId });

            builder.HasOne(de => de.Directory)
                    .WithMany(d => d.DocumentDirectoryExtensions)
                    .HasForeignKey(de => de.DirectoryId);

            builder.HasOne(de => de.Extension)
                 .WithMany(e => e.DocumentDirectoryExtensions)
                 .HasForeignKey(de => de.ExtensionId);

            Seed(builder);
        }
        
        protected virtual void Seed(EntityTypeBuilder<DocumentDirectoryExtension> builder)
        {
            if (!_documentExtensions.HasItems())
                return;

            var enumValues = (TDirectoryEnumType[])Enum.GetValues(typeof(TDirectoryEnumType));
            if (enumValues == null || enumValues.Length == 0)
                return;
            
            var documentDirectoryExtensionSeedItems = new List<DocumentDirectoryExtension>();

            foreach (var enumValue in enumValues)
            {
                if (!_directoryBuilderLookup.TryGetValue(enumValue, out DocumentDirectoryBuilder<TDirectoryEnumType> directoryBuilder))
                {
                    throw new InvalidOperationException($@"Directory builder was not found for {typeof(TDirectoryEnumType).FullName} value of {enumValue}. 
A directory builder must be applied at startup for all Document Directory enum types.");
                }
                    
                if (directoryBuilder.Extensions != null)
                {
                    // if extensions were specified, add those for the directory enum
                    foreach (var ext in directoryBuilder.Extensions)
                    {
                        // NOTE: extension will need to exist in master list of seeded extensions prior to adding extension to the directory
                        if (!_documentExtensions.TryGetValue(ext, out DocumentExtension extension))
                        {
                            throw new InvalidOperationException($@"Error seeding Document Directory Extensions. 
Extension '{ext}' was not found in available extensions. 
Please add file extension to supplied list via the {typeof(FileExtensions).FullName} class passed to service collection registration.");
                        }
                            
                        documentDirectoryExtensionSeedItems.Add(new DocumentDirectoryExtension(enumValue.ToInt(), extension.Id));
                    }
                }
            }

            if (documentDirectoryExtensionSeedItems.Count != 0)
                builder.HasData(documentDirectoryExtensionSeedItems);
        }
    }
}
