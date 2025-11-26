using Common.Core;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Common.EntityFrameworkCore
{
    public static class ModelBuilderExtenions
    {
        /// <summary>
        /// Searches <paramref name="assembly"/> for all <see cref="IEntityTypeConfiguration{TEntity}"/> types that have valid implementations.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        internal static IEnumerable<Type> GetEFConfigurationTypes(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t =>
                                !t.IsAbstract
                                && !t.IsGenericTypeDefinition
                                && t.GetTypeInfo().ImplementedInterfaces.Any(i =>
                                        i.IsGenericType
                                        && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));
        }        

        /// <summary>
        /// Adds all applicable configurations of <see cref="IEntityTypeConfiguration{TEntity}"/> found in the assembly of the provided DbContext type.
        /// </summary>
        /// <typeparam name="TContext">DbContext type. The assembly for this type is what is scanned for configuration classes.</typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="predicate">Optional predicate to filter types that are found.</param>
        /// <returns></returns>
        public static ModelBuilder ApplyConfigurationsFromAssembly<TContext>(
            this ModelBuilder modelBuilder,
            Func<Type, bool>? predicate = null)
            where TContext : DbContext
        {
            Assembly? assembly = Assembly.GetAssembly(typeof(TContext));
            if (assembly != null)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly, predicate);
            }
            return modelBuilder;
        }        

        /// <summary>
        /// Pluralizes entity table names on the <paramref name="modelBuilder"/>. 
        /// Uses <see cref="Humanizer"/> to perform pluralization.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder PluralizeTableNames(this ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetTableEntityTypes();
            foreach (var entityType in entityTypes)
            {
                string? tableName = entityType.GetTableName();
                entityType.SetTableName(tableName.Pluralize(inputIsKnownToBeSingular: false));
            }

            return modelBuilder;
        }

        /// <summary>
        /// Singularizes entity table names on the <paramref name="modelBuilder"/>. 
        /// Uses <see cref="Humanizer"/> to perform singularization.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder SingularizeTableNames(this ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetTableEntityTypes();
            foreach (var entityType in entityTypes)
            {
                string? tableName = entityType.GetTableName();
                entityType.SetTableName(tableName.Singularize(inputIsKnownToBePlural: false));
            }

            return modelBuilder;
        }

        /// <summary>
        /// Sets all entity database tables and columns to a snake_case syntax .
        /// Uses <see cref="Humanizer"/> to set case syntax.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder UseSnakeCase(this ModelBuilder modelBuilder)
        {
            return UseCase(modelBuilder, EntityFormatCaseOption.SnakeCase);
        }

        /// <summary>
        /// Sets all entity database tables and columns to use a defined case syntax. Options include snake_case, PascalCase, camelCase, and Titlecase.
        /// Uses <see cref="Humanizer"/> to set case syntax.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="caseFormat"></param>
        /// <returns></returns>
        public static ModelBuilder UseCase(this ModelBuilder modelBuilder, EntityFormatCaseOption caseFormat)
        {
            // Get "table" entities, or entities that map directly to tables.
            // This omits complextype/valueobject entities intended to be "owned" by main entities.
            // Only "table" entities are used because setting the TableName annotation throws off
            // valueobject mapping on parent tables.
            var entityTypes = modelBuilder.Model.GetTableEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var tableName = entityType.GetTableName();
                entityType.SetTableName(ToCase(caseFormat, tableName!));
            }

            var alltypes = modelBuilder.Model.GetEntityTypes();

            foreach (var entityType in alltypes)
            {
                var properties = entityType.GetProperties();
                foreach (var property in properties)
                {
                    var columnName = property.GetColumnName(StoreObjectIdentifier.Table(entityType.GetTableName()!, ((IMutableEntityType)property.DeclaringType).GetSchema()));
                    property.SetColumnName(ToCase(caseFormat, columnName!));
                }
            }

            return modelBuilder;
        }

        internal static string ToCase(EntityFormatCaseOption caseFormat, string input)
        {
            return caseFormat switch
            {
                EntityFormatCaseOption.SnakeCase => input.Underscore(),
                EntityFormatCaseOption.PascalCase => input.Pascalize(),
                EntityFormatCaseOption.CamelCase => input.Camelize(),
                EntityFormatCaseOption.TitleCase => input.Titleize(),
                _ => throw new UnsupportedEnumException(caseFormat)
            };
        }
    }
}
