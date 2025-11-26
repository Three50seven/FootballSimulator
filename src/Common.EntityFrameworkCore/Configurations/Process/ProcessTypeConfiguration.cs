using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Core.Domain;
using System;

namespace Common.EntityFrameworkCore
{
    public class ProcessTypeConfiguration<TEnum> : IEntityTypeConfiguration<ProcessType>
        where TEnum : Enum
    {
        public void Configure(EntityTypeBuilder<ProcessType> builder)
        {
            builder.ConfigureLookupEntityProperties<ProcessType, TEnum>(type => new ProcessType(type));
        }
    }
}
