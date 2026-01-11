using Common.EntityFrameworkCore;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class QualificationTypeConfiguration : IEntityTypeConfiguration<QualificationType>
    {
        public void Configure(EntityTypeBuilder<QualificationType> builder)
        {
            builder.ConfigureLookupEntityProperties<QualificationType, QualificationTypeOption>(type => new QualificationType(type));
        }
    }
}