using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class ProcessParameterConfiguration : IEntityTypeConfiguration<ProcessParameter>
    {
        public void Configure(EntityTypeBuilder<ProcessParameter> builder)
        {
            builder.ConfigureKeyValueProperties();

            builder.HasOne(par => par.Process).WithMany(p => p.Parameters).HasForeignKey(par => par.ProcessId);
        }
    }
}
