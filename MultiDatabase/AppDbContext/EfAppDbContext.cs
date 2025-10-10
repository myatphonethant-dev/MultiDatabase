namespace MultiDatabase.AppDbContext;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DatabaseType
{
    Mssql,
    Postgres
}

public interface IDbContextFactory
{
    DbContext CreateDbContext();
}

public class DbContextFactory : IDbContextFactory
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public DbContextFactory(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    public DbContext CreateDbContext()
    {
        string databaseType = _configuration["DatabaseType"] ?? "Mssql";
        var enumDbType = Enum.Parse<DatabaseType>(databaseType);

        switch (enumDbType)
        {
            case DatabaseType.Mssql:
                return _serviceProvider.GetRequiredService<SqlServerDbContext>();

            case DatabaseType.Postgres:
                return _serviceProvider.GetRequiredService<PostgresDbContext>();

            default:
                throw new ArgumentException($"Unsupported database type: {databaseType}");
        }
    }
}

public interface IDatabaseContextFactory
{
    DbContext CreateDbContext(string databaseName, DatabaseType databaseType = DatabaseType.Mssql);
    Task<bool> TestConnectionAsync(string databaseName, DatabaseType databaseType = DatabaseType.Mssql);
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

    public DbContext CreateDbContext(string databaseName, DatabaseType databaseType = DatabaseType.Mssql)
    {
        switch (databaseType)
        {
            case DatabaseType.Mssql:
                var sqlServerConn = GetSqlServerConnectionString(_sqlServerConnectionString, databaseName);
                var sqlServerOptions = new DbContextOptionsBuilder<SqlServerDbContext>()
                    .UseSqlServer(sqlServerConn)
                    .Options;
                return new SqlServerDbContext(sqlServerOptions);

            case DatabaseType.Postgres:
                var postgresConn = GetPostgresConnectionString(_postgresConnectionString, databaseName);
                var postgresOptions = new DbContextOptionsBuilder<PostgresDbContext>()
                    .UseNpgsql(postgresConn)
                    .Options;
                return new PostgresDbContext(postgresOptions);

            default:
                throw new ArgumentException($"Unsupported database type: {databaseType}");
        }
    }

    public async Task<bool> TestConnectionAsync(string databaseName, DatabaseType databaseType = DatabaseType.Postgres)
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