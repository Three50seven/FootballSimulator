using Common.Core.Domain;
using Common.EntityFrameworkCore;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> ConfigureFSDataEntityProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
           where TEntity : FSDataEntity, IDomainEntity
        {
            builder.ConfigureDomainEntityProperties(includeConcurrency: true);
            builder.OwnsOne(a => a.ChangeEvents, changeEventsBuilder => changeEventsBuilder.ConfigureChangeEventsProperties<TEntity, User>());

            return builder;
        }
    }
}
