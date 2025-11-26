using Common.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.EntityFrameworkCore
{
    public static class KeyValueEntityBuilderExtensions
    {
        /// <summary>
        /// Configure standard settings for the key/value properties on a <see cref="KeyValueEntity"/>.
        /// Key is required and set to 200 lengh. Value is optionally required.
        /// </summary>
        /// <typeparam name="T">KeyValueEntity type.</typeparam>
        /// <param name="builder"></param>
        /// <param name="valueRequired">Whether the value column <see cref="KeyValueEntity.Value"/> is required. Defaults to false.</param>
        /// <returns></returns>
        public static EntityTypeBuilder<T> ConfigureKeyValueProperties<T>(
           this EntityTypeBuilder<T> builder,
           bool valueRequired = false)
           where T : KeyValueEntity
        {
            builder.Property(kv => kv.Key).IsRequired().HasMaxLength(200);
            builder.Property(kv => kv.Value).IsRequired(valueRequired);

            return builder;
        }
    }
}
