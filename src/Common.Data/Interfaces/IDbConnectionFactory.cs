using Common.Core;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Common.Data
{
    public interface IDbConnectionFactory 
        : IDbConnectionFactory<SqlConnection>
    {

    }

    public interface IDbConnectionFactory<TConn> 
        : IFactory<TConn> where TConn : class, IDbConnection
    {
    }
}
