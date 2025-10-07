using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MultiDatabase.Models.Postgres;
using MultiDatabase.Models.SqlServer;

namespace MultiDatabase.AppDbContext
{
    public enum DatabaseType
    {
        SqlServer,
        PostgreSQL
    }

    public interface IDatabaseContextFactory
    {
        DbContext CreateDbContext(string databaseName, DatabaseType databaseType = DatabaseType.SqlServer);
        Task<bool> TestConnectionAsync(string databaseName, DatabaseType databaseType = DatabaseType.SqlServer);
    }

    public class DatabaseContextFactory : IDatabaseContextFactory
    {
        private readonly string _sqlServerConnectionString;
        private readonly string _postgresConnectionString;

        public DatabaseContextFactory(IConfiguration configuration)
        {
            _sqlServerConnectionString = configuration.GetConnectionString("SqlServerConnection")!;
            _postgresConnectionString = configuration.GetConnectionString("PostgresConnection")!;
        }

        public DbContext CreateDbContext(string databaseName, DatabaseType databaseType = DatabaseType.SqlServer)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    var sqlServerConn = GetSqlServerConnectionString(_sqlServerConnectionString, databaseName);
                    var sqlServerOptions = new DbContextOptionsBuilder<SqlServerDbContext>()
                        .UseSqlServer(sqlServerConn)
                        .Options;
                    return new SqlServerDbContext(sqlServerOptions);

                case DatabaseType.PostgreSQL:
                    var postgresConn = GetPostgresConnectionString(_postgresConnectionString, databaseName);
                    var postgresOptions = new DbContextOptionsBuilder<PostgresDbContext>()
                        .UseNpgsql(postgresConn)
                        .Options;
                    return new PostgresDbContext(postgresOptions);

                default:
                    throw new ArgumentException($"Unsupported database type: {databaseType}");
            }
        }

        public async Task<bool> TestConnectionAsync(string databaseName, DatabaseType databaseType = DatabaseType.SqlServer)
        {
            try
            {
                using var context = CreateDbContext(databaseName, databaseType);
                await context.Database.CanConnectAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetSqlServerConnectionString(string connectionString, string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
                return connectionString;

            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = databaseName
            };
            return builder.ConnectionString;
        }

        private static string GetPostgresConnectionString(string connectionString, string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
                return connectionString;

            var builder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString)
            {
                Database = databaseName
            };
            return builder.ConnectionString;
        }
    }
}