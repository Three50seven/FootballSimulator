using Common.Core;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Common.EntityFrameworkCore
{
    public static class ModelBuilderExtenions
    {
        //
        // Summary:
        //     Searches assembly for all Microsoft.EntityFrameworkCore.IEntityTypeConfiguration`1
        //     types that have valid implementations.
        //
        // Parameters:
        //   assembly:
        internal static IEnumerable<Type> GetEFConfigurationTypes(this Assembly assembly)
        {
            return from t in assembly.GetTypes()
                   where !t.IsAbstract && !t.IsGenericTypeDefinition && t.GetTypeInfo().ImplementedInterfaces.Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                   select t;
        }
        
        //
        // Summary:
        //     Adds all applicable configurations of Microsoft.EntityFrameworkCore.IEntityTypeConfiguration`1
        //     found in the assembly of the provided DbContext type.
        //
        // Parameters:
        //   modelBuilder:
        //
        //   predicate:
        //     Optional predicate to filter types that are found.
        //
        // Type parameters:
        //   TContext:
        //     DbContext type. The assembly for this type is what is scanned for configuration
        //     classes.
        public static ModelBuilder ApplyConfigurationsFromAssembly<TContext>(this ModelBuilder modelBuilder, Func<Type, bool>? predicate = null) where TContext : DbContext
        {
            Assembly? assembly = Assembly.GetAssembly(typeof(TContext));
            if (assembly != null)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly, predicate);
            }
            return modelBuilder;
        }

        //
        // Summary:
        //     Pluralizes entity table names on the modelBuilder. Uses Humanizer to perform
        //     pluralization.
        //
        // Parameters:
        //   modelBuilder:
        public static ModelBuilder PluralizeTableNames(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                // Skip entity types that don't map to a table (e.g., owned types)
                if (entity.IsOwned()) continue;

                var currentTableName = entity.GetTableName();
                if (!string.IsNullOrEmpty(currentTableName))
                {
                    var pluralName = currentTableName.Pluralize(inputIsKnownToBeSingular: false);
                    entity.SetTableName(pluralName);
                }
            }

            return modelBuilder;
        }

        //
        // Summary:
        //     Singularizes entity table names on the modelBuilder. Uses Humanizer to perform
        //     singularization.
        //
        // Parameters:
        //   modelBuilder:
        public static ModelBuilder SingularizeTableNames(this ModelBuilder modelBuilder)
        {
            IEnumerable<IMutableEntityType> tableEntityTypes = modelBuilder.Model.GetTableEntityTypes();
            foreach (IMutableEntityType item in tableEntityTypes)
            {
                string? tableName = item.GetTableName();
                item.SetTableName(tableName.Singularize(inputIsKnownToBePlural: false));
            }

            return modelBuilder;
        }

        //
        // Summary:
        //     Sets all entity database tables and columns to a snake_case syntax . Uses Humanizer
        //     to set case syntax.
        //
        // Parameters:
        //   modelBuilder:
        public static ModelBuilder UseSnakeCase(this ModelBuilder modelBuilder)
        {
            return modelBuilder.UseCase(EntityFormatCaseOption.SnakeCase);
        }

        //
        // Summary:
        //     Sets all entity database tables and columns to use a defined case syntax. Options
        //     include snake_case, PascalCase, camelCase, and Titlecase. Uses Humanizer to set
        //     case syntax.
        //
        // Parameters:
        //   modelBuilder:
        //
        //   caseFormat:
        public static ModelBuilder UseCase(this ModelBuilder modelBuilder, EntityFormatCaseOption caseFormat)
        {
            IEnumerable<IMutableEntityType> tableEntityTypes = modelBuilder.Model.GetTableEntityTypes();
            foreach (IMutableEntityType item in tableEntityTypes)
            {
                string? tableName = item.GetTableName();
                item.SetTableName(ToCase(caseFormat, tableName ?? string.Empty));
            }

            IEnumerable<IMutableEntityType> entityTypes = modelBuilder.Model.GetEntityTypes();
            foreach (IMutableEntityType item2 in entityTypes)
            {
                IEnumerable<IMutableProperty> properties = item2.GetProperties();
                foreach (IMutableProperty item3 in properties)
                {
                    string? tableName = item2.GetTableName();
                    string? schema = ((IMutableEntityType)item3.DeclaringType).GetSchema();

                    if (tableName is null)
                        continue; // or handle with a fallback name

                    StoreObjectIdentifier storeObject = StoreObjectIdentifier.Table(tableName, schema);

                    string? columnName = item3.GetColumnName(storeObject);
                    if (columnName is null)
                        continue; // or use a fallback like item3.Name

                    item3.SetColumnName(ToCase(caseFormat, columnName));
                }
            }

            return modelBuilder;
        }

        internal static string ToCase(EntityFormatCaseOption caseFormat, string input)
        {
            if (1 == 0)
            {
            }

            string result = caseFormat switch
            {
                EntityFormatCaseOption.SnakeCase => input.Underscore(),
                EntityFormatCaseOption.PascalCase => input.Pascalize(),
                EntityFormatCaseOption.CamelCase => input.Camelize(),
                EntityFormatCaseOption.TitleCase => input.Titleize(),
                _ => throw new UnsupportedEnumException(caseFormat),
            };
            if (1 == 0)
            {
            }

            return result;
        }
    }
}
