using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Common.Core.Validation;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Common.EntityFrameworkCore
{
    public static class DatabaseFacadeExtensions
    {
        /// <summary>
        /// Executes TRUNCATE TABLE on provided table name.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static Task<int> TruncateTableAsync(this DatabaseFacade database, string table)
        {
            Guard.IsNotNull(table, nameof(table));

            return database.ExecuteSqlInterpolatedAsync($"TRUNCATE TABLE [{table}]");
        }

        /// <summary>
        /// Executes ALTER TABLE ... DROP CONSTRAINT using provided values.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table">Table housing the constraint.</param>
        /// <param name="constraint">Name of the constraint.</param>
        /// <param name="withOnlineOff">Whether or not WITH ( ONLINE = OFF ) is appende to the statement. Default is false.</param>
        /// <returns></returns>
        public static Task<int> DropConstraintAsync(this DatabaseFacade database, string table, string constraint, bool withOnlineOff = false)
        {
            Guard.IsNotNull(table, nameof(table));
            Guard.IsNotNull(constraint, nameof(constraint));

            string sql = $"ALTER TABLE [dbo].[{table}] DROP CONSTRAINT [{constraint}]";

            if (withOnlineOff)
                sql += " WITH ( ONLINE = OFF )";

            return database.ExecuteSqlRawAsync(sql);
        }

        /// <summary>
        /// Executes a SqlBulkCopy operation to insert the datatable using the underlying db connection.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static async Task WriteSqlBulkCopyAsync(this DatabaseFacade database, DataTable table)
        {
            Guard.IsNotNull(table, nameof(table));

            var conn = (SqlConnection)database.GetDbConnection() ?? throw new InvalidOperationException("SqlConnection not found.");
            await conn.OpenAsync();

            using var sqlBulkCopy = new SqlBulkCopy(conn);
            sqlBulkCopy.DestinationTableName = table.TableName;

            foreach (DataColumn item in table.Columns)
            {
                sqlBulkCopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);
            }

            await sqlBulkCopy.WriteToServerAsync(table);
        }
    }
}
