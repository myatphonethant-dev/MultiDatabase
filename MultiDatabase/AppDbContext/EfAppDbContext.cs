using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MultiDatabase.AppDbContext
{
    public class EfAppDbContext : DbContext
    {
        public EfAppDbContext(DbContextOptions<EfAppDbContext> options) : base(options)
        {
        }
    }

    public enum DatabaseType
    {
        SqlServer,
        PostgreSQL
    }

    public interface IDatabaseContextFactory
    {
        EfAppDbContext CreateDbContext(string databaseName, DatabaseType databaseType = DatabaseType.SqlServer);
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

        public EfAppDbContext CreateDbContext(string databaseName, DatabaseType databaseType = DatabaseType.SqlServer)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfAppDbContext>();

            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    var sqlServerConn = GetSqlServerConnectionString(_sqlServerConnectionString, databaseName);
                    optionsBuilder.UseSqlServer(sqlServerConn);
                    break;

                case DatabaseType.PostgreSQL:
                    var postgresConn = GetPostgresConnectionString(_postgresConnectionString, databaseName);
                    optionsBuilder.UseNpgsql(postgresConn);
                    break;

                default:
                    throw new ArgumentException($"Unsupported database type: {databaseType}");
            }

            return new EfAppDbContext(optionsBuilder.Options);
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

            var builder = new NpgsqlConnectionStringBuilder(connectionString)
            {
                Database = databaseName
            };
            return builder.ConnectionString;
        }
    }
}