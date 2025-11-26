using Dapper;
using Microsoft.Data.SqlClient;

namespace Common.Data
{
    public class SqlFileExecutor : ISqlFileExecutor
    {
        private readonly string _connString;

        public SqlFileExecutor(string connString)
        {
            _connString = connString;
        }

        public void DropExecuteStoredProcedure(string storedProcName, string filePath, int timeout = 60)
        {
            if (string.IsNullOrWhiteSpace(storedProcName))
                throw new ArgumentNullException(nameof(storedProcName));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            var storedProcCreateSql = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(storedProcCreateSql))
                throw new InvalidOperationException($"File intended to contain Stored Procedure ({storedProcName}) create SQL script is empty. File path: '{filePath}'.");

            using var conn = new SqlConnection(_connString);
            conn.Open();

            // drop and recreate stored proc every time so that the SQL file holds the latest logic
            conn.Execute(SqlStatements.DropStoredProceedure(storedProcName));

            // create stored proc
            conn.Execute(storedProcCreateSql);

            // execute
            conn.Execute($"EXEC [{storedProcName}]", commandTimeout: timeout);

            conn.Close();
        }
    }
}
