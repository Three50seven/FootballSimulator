using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class ProcessRetryConfiguration : IEntityTypeConfiguration<ProcessRetry>
    {
        public void Configure(EntityTypeBuilder<ProcessRetry> builder)
        {
            builder.Property(pr => pr.Reason).IsRequired(false);
            builder.HasOne(pr => pr.Process).WithMany(p => p.Retries).HasForeignKey(pr => pr.ProcessId);
        }
    }
}
