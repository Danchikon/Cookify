using Cookify.Api;
using Cookify.Api.Extensions;
using Cookify.Application;
using Cookify.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFiles();

builder.Host.ConfigureSerilog();

builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.UseErrorHandlerMiddleware();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.MapControllers();

await app.RunAsync();