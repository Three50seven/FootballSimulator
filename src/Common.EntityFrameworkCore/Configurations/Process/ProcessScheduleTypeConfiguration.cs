using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core;
using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public class ProcessScheduleTypeConfiguration : IEntityTypeConfiguration<ProcessScheduleType>
    {
        public void Configure(EntityTypeBuilder<ProcessScheduleType> builder)
        {
            builder.ConfigureLookupEntityProperties<ProcessScheduleType, ProcessScheduleTypeOption>(type => new ProcessScheduleType(type));
        }
    }
}
