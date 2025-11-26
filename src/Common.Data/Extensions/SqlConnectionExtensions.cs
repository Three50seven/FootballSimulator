using System.Data;
using Microsoft.Data.SqlClient;

namespace Common.Data
{
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Sends SQL command query <paramref name="sql"/> to the database 
        /// and returns a filled <see cref="DataTable"/> from the results.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable QueryDataTable(this SqlConnection conn, string sql)
        {
            ArgumentNullException.ThrowIfNull(conn);

            using var cmd = new SqlCommand(sql, conn);
            using var adapter = new SqlDataAdapter(cmd);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
