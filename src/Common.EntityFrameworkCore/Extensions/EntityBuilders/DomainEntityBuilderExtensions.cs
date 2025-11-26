using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public static class DomainEntityBuilderExtensions
    {
        /// <summary>
        /// Set standard property configuration for DomainEntity types. 
        /// Includes Id key, default value for guid (NEWID()) and concurrency option (row version ignored by default).
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="includeConcurrency">Whether <see cref="IEntityConcurrency.RowVersion"/> should be set as the SQL row version or be ignored.</param>
        /// <returns></returns>
        public static EntityTypeBuilder<TEntity> ConfigureDomainEntityProperties<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            bool includeConcurrency = false)
            where TEntity : class, IDomainEntity, IEntityConcurrency
        {
            return ConfigureDomainEntityProperties<TEntity, int>(builder, includeConcurrency);
        }

        /// <summary>
        /// Set standard property configuration for DomainEntity types. 
        /// Includes Id key, default value for guid (NEWID()) and concurrency option (row version ignored by default).
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="builder"></param>
        /// <param name="includeConcurrency">Whether <see cref="IEntityConcurrency.RowVersion"/> should be set as the SQL row version or be ignored.</param>
        /// <returns></returns>
        public static EntityTypeBuilder<TEntity> ConfigureDomainEntityProperties<TEntity, TKey>(
            this EntityTypeBuilder<TEntity> builder,
            bool includeConcurrency = false)
            where TEntity : class, IDomainEntity<TKey>, IEntityConcurrency
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Guid).IsRequired().HasDefaultValueSql("NEWID()");

            builder.IncludeConcurrency(includeConcurrency);

            return builder;
        }

        /// <summary>
        /// Set <see cref="IEntityConcurrency.RowVersion"/> as the row version colum or ignore.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="include">Whether to include row version or ignore.</param>
        /// <returns></returns>
        public static EntityTypeBuilder<T> IncludeConcurrency<T>(
            this EntityTypeBuilder<T> builder,
            bool include = true)
            where T : class, IEntityConcurrency
        {
            if (include)
                builder.Property(x => x.RowVersion).IsRowVersion();
            else
                builder.Ignore(x => x.RowVersion);

            return builder;
        }
    }
}
