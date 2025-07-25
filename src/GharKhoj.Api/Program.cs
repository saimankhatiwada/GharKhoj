using GharKhoj.Api;
using GharKhoj.Api.Extensions;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddApiServices();

WebApplication app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.MapScalarApiReference(options =>
{
    options.WithTheme(ScalarTheme.BluePlanet);
    options.WithOpenApiRoutePattern("/swagger/1.0/swagger.json");
});

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

public partial class Program;
