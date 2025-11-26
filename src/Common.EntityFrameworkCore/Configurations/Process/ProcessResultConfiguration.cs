using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class ProcessResultConfiguration : IEntityTypeConfiguration<ProcessResult>
    {
        public void Configure(EntityTypeBuilder<ProcessResult> builder)
        {
            builder.ConfigureKeyValueProperties();
            builder.HasOne(par => par.Process).WithMany(p => p.Results).HasForeignKey(par => par.ProcessId);
        }
    }
}
