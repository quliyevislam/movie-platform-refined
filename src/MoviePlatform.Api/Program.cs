using MoviePlatform.Application;
using MoviePlatform.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// dotnet ef migrations add <migration-name> --project src/MoviePlatform.Infrastructure --startup-project src/MoviePlatform.Api
// psql -h localhost -p 5432 -U postgres -d movie_platform_db
