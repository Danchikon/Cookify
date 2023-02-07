using System.Net;
using Cookify.Api.Extensions;
using Cookify.Application;
using Cookify.Domain.Common.Exceptions;
using Cookify.Infrastructure;
using Cookify.Infrastructure.Common.Helpers;
using Cookify.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFiles();

builder.Host.ConfigureSerilog();

builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

await app.UseEfDatabaseAsync<CookifyDbContext>(default);

if (!AspNetCoreEnvironment.IsProduction)
{
    await app.UseMinioStorageAsync(default);
}

app.UseErrorHandlerMiddleware(exception => exception switch
{
    BusinessExceptionBase => HttpStatusCode.Conflict,
    _ => HttpStatusCode.InternalServerError
});

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
