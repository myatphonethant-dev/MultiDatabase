dotnet ef dbcontext scaffold "Server=localhost;Database=TestDb;User Id=sa;Password=sasa@123;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer -o Models/SqlServer -c SqlServerDbContext --force

dotnet ef dbcontext scaffold "Host=localhost;Database=TestDb;Username=postgres;Password=sasa@123" Npgsql.EntityFrameworkCore.PostgreSQL -o Models/Postgres -c PostgresDbContext --force