using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ConfigureDomainEntityProperties();

            builder.Property(x => x.Name).HasMaxLength(500).IsRequired();
            builder.Property(x => x.SubPath).IsRequired(false);
            builder.OwnsOne(x => x.File, fileBuilder => fileBuilder.ConfigureFileDetailProperties());

            builder.HasOne(x => x.Directory).WithMany().HasForeignKey(x => x.DirectoryId);
        }
    }
}
