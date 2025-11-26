using Dapper;
using Microsoft.Data.SqlClient;
using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using System.Data;

namespace Common.Data
{
    public class FileSourceSqlRepository : IFileSourceRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public FileSourceSqlRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public virtual async Task AddOrUpdateAsync(FileSource file)
        {
            Guard.IsNotNull(file, nameof(file));

            using var conn = _dbConnectionFactory.Create();
            using var cmd = new SqlCommand("INSERT INTO [files] ([id], [path], [source]) VALUES (@guid, @path, @source)", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@guid", file.Id);
            cmd.Parameters.AddWithValue("@path", file.Path);
            cmd.Parameters.Add(new SqlParameter("@source", SqlDbType.Image) { Value = file.Source });

            await cmd.ExecuteNonQueryAsync();
        }

        public virtual async Task DeleteAsync(string path)
        {
            Guard.IsNotNull(path, nameof(path));

            using var conn = _dbConnectionFactory.Create();
            using var cmd = new SqlCommand("DELETE FROM [files] WHERE [path] = @path", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@path", path);

            await cmd.ExecuteNonQueryAsync();
        }

        public virtual async Task<bool> ExistsAsync(string path)
        {
            Guard.IsNotNull(path, nameof(path));

            using var conn = _dbConnectionFactory.Create();
            using var cmd = new SqlCommand("SELECT TOP 1 [id] FROM [files] WHERE [path] = @path", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@path", path);

            var result = (Guid?)await cmd.ExecuteScalarAsync();
            return result != null && result != Guid.Empty;
        }

        public virtual async Task<FileSource> GetByPathAsync(string path)
        {
            using var conn = _dbConnectionFactory.Create();
            return await conn.QuerySingleOrDefaultAsync<FileSource>("SELECT [id], [path], [source] FROM [files] WHERE [path] = @path;");
        }
    }
}
