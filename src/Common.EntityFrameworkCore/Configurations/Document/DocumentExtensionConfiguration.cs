using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core;
using Common.Core.Domain;
using System.Collections.Generic;

namespace Common.EntityFrameworkCore
{
    public class DocumentExtensionConfiguration : IEntityTypeConfiguration<DocumentExtension>
    {
        private readonly IEnumerable<DocumentExtension> _documentExtensionSeedItems;

        public DocumentExtensionConfiguration(IEnumerable<DocumentExtension> documentExtensionSeedItems)
        {
            _documentExtensionSeedItems = documentExtensionSeedItems;
        }

        public void Configure(EntityTypeBuilder<DocumentExtension> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Extension).HasMaxLength(50).IsRequired();
            builder.Property(e => e.MimeType).HasMaxLength(500).IsRequired();

            builder.Ignore(e => e.Directories);
            builder.HasIndex(e => e.Extension).IsUnique().HasDatabaseName("IX_document_extension_extension");

            if (_documentExtensionSeedItems.HasItems())
                builder.HasData(_documentExtensionSeedItems);
        }
    }
}
