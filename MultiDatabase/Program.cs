using MultiDatabase.AppDbContext;
using MultiDatabase.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDatabaseContextFactory, DatabaseContextFactory>();
builder.Services.AddScoped<IMultiDatabaseServiceFactory, MultiDatabaseServiceFactory>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();