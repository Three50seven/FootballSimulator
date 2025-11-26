using Microsoft.Data.SqlClient;

namespace Common.Data
{
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connString;

        public SqlDbConnectionFactory(string connString)
        {
            if (string.IsNullOrWhiteSpace(connString))
                throw new ArgumentNullException(nameof(connString));

            _connString = connString;
        }

        public virtual SqlConnection Create()
        {
            return new SqlConnection(_connString);
        }
    }
}
