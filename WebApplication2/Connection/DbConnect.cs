using System.Data;
using Dapper;
using Npgsql;

namespace WebApplication2.Connection
{
    public static class DbConnect
    {
        private static readonly string connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123123";

        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(sql, param);
        }

        public static async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public static async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, param);
        }
    }
}
