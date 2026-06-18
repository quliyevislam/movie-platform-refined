using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;
using MoviePlatform.Application.Common.Data;

namespace MoviePlatform.Infrastructure.Persistence;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Database connection string is missing or invalid.");
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
