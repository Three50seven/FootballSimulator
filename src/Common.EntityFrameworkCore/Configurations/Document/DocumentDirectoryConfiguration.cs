using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Collections.Generic;

namespace Common.EntityFrameworkCore
{
    public class DocumentDirectoryConfiguration<TEnumType> : IEntityTypeConfiguration<DocumentDirectory>
        where TEnumType : Enum
    {
        private readonly IDictionary<TEnumType, DocumentDirectoryBuilder<TEnumType>> _directoryBuilderLookup;
        private readonly string _root;

        public DocumentDirectoryConfiguration(
            IDictionary<TEnumType, DocumentDirectoryBuilder<TEnumType>> directoryBuilderLookup, 
            string root = "/")
        {
            Guard.IsNotNull(directoryBuilderLookup, nameof(directoryBuilderLookup));

            _directoryBuilderLookup = directoryBuilderLookup;

            _root = string.IsNullOrWhiteSpace(root) ? "/" : root.Trim().ToLower();
            if (!_root.EndsWith('/'))
                _root += "/";
        }

        public void Configure(EntityTypeBuilder<DocumentDirectory> builder)
        {
            builder.ConfigureLookupEntityProperties<DocumentDirectory, TEnumType>(CreateSeedValue);
            builder.Property(d => d.Path).IsRequired();
            builder.Ignore(d => d.Extensions);
        }

        protected virtual DocumentDirectory CreateSeedValue(TEnumType enumValue)
        {
            if (!_directoryBuilderLookup.TryGetValue(enumValue, out DocumentDirectoryBuilder<TEnumType> directoryBuilder))
                throw new InvalidOperationException($@"Directory builder was not found for {typeof(TEnumType).FullName} value of {enumValue}. 
A directory builder must be applied at startup for all Document Directory enum types.");

            return new DocumentDirectory(enumValue,
                                         path: (directoryBuilder.IncludeRoot ? _root : "") + directoryBuilder.Path,
                                         maxFileSize: directoryBuilder.MaxFileSize);
        }
    }
}
