using Cookify.Api.Extensions;
using Cookify.Application;
using Cookify.Infrastructure;
using Cookify.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFiles();

builder.Host.ConfigureSerilog();

builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

await app.UseDatabaseAsync<CookifyDbContext>();

app.UseErrorHandlerMiddleware();

app.UseResponseCompression();

app.UseCors(configurePolicy => 
    {
        configurePolicy.AllowAnyOrigin();
        configurePolicy.AllowAnyHeader();
        configurePolicy.AllowAnyMethod();
    }
);

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();

app.MapControllers();

await app.RunAsync();



var Бандера = new {};