var builder = WebApplication.CreateBuilder(args);

string databaseType = builder.Configuration["DatabaseType"] ?? "Mssql";
var enumDbType = Enum.Parse<DatabaseType>(databaseType);
builder.Services.AddSingleton(typeof(DatabaseType), enumDbType);

switch (enumDbType)
{
    case DatabaseType.Mssql:
        builder.Services.AddDbContext<SqlServerDbContext>(opt =>
        {
            var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
            opt.UseSqlServer(connectionString);
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);
        break;

    case DatabaseType.Postgres:
        builder.Services.AddDbContext<PostgresDbContext>(opt =>
        {
            var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
            opt.UseNpgsql(connectionString);
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);
        break;

    default:
        throw new ArgumentException($"Unsupported database type: {databaseType}");
}

builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();

builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();

/*
builder.Services.AddScoped<IDatabaseContextFactory, DatabaseContextFactory>();
builder.Services.AddScoped<IMultiDatabaseServiceFactory, MultiDatabaseServiceFactory>();*/

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();