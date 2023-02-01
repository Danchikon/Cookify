using Cookify.Api;
using Cookify.Api.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFiles();
builder.Host.ConfigureSerilog();
builder.Services.AddApiServices();

var app = builder.Build();

app.UseErrorHandlerMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.MapControllers();

await app.RunAsync();