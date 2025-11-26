using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Common.Data
{
    public static class SqlStatements
    {
        public static string ColumnsSelect<T>(bool underscore = true) where T : class
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length == 0)
                throw new InvalidOperationException($"Type {typeof(T).FullName} does not have any public properties to be used on generating SQL select statement.");

            var columnNames = new List<string>();

            for (int i = 0; i < properties.Length; i++)
            {
                var attr = properties[i].GetCustomAttribute<DisplayColumnAttribute>();
                if (attr != null)
                    columnNames.Add(attr.DisplayColumn);
                else
                    columnNames.Add(underscore ? properties[i].Name.Underscore() : properties[i].Name);
            }

            return string.Join(",", columnNames.Select(col => $"[{col}]").ToArray());
        }

        public static string DropTable(string tableName, string schema = "dbo")
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException(nameof(tableName));

            return $@"
                IF (EXISTS (SELECT * 
                            FROM INFORMATION_SCHEMA.TABLES 
                            WHERE TABLE_SCHEMA = '{schema}' 
                            AND TABLE_NAME = '{tableName}'))
                BEGIN
                    DROP TABLE [{tableName}]
                END";
        }

        public static string DropStoredProceedure(string storedProcName)
        {
            if (string.IsNullOrWhiteSpace(storedProcName))
                throw new ArgumentNullException(nameof(storedProcName));

            return $@"
                IF (EXISTS (SELECT * 
                            FROM sys.objects 
                            WHERE object_id = OBJECT_ID('{storedProcName}') 
                                AND type IN (N'P', N'PC')))
                BEGIN
	                DROP PROCEDURE [{storedProcName}]
                END";
        }
    }
}
