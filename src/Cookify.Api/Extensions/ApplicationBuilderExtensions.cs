using System.Net;
using Cookify.Api.Common.Helpers;
using Cookify.Api.Middlewares;
using Cookify.Infrastructure.Common.Seeders;
using Cookify.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Cookify.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    #region Error Handler Middleware
    
    public static IApplicationBuilder UseErrorHandlerMiddleware(
        this IApplicationBuilder app, 
        Func<Exception, HttpStatusCode>? customMap = null
    )
    {
        ExceptionToObjectResultConverter.CustomMap = customMap;
        return app.UseMiddleware<ErrorHandlerMiddleware>();
    }
    
    #endregion

    #region Swagger

    public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder)
    {
        var options = builder.ApplicationServices.GetRequiredService<IOptions<SwaggerOptions>>().Value;
        
        if (!options.Enabled)
        {
            return builder;
        }

        var routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "swagger" : options.RoutePrefix;

        builder
            .UseStaticFiles()
            .UseSwagger(swaggerOptions => swaggerOptions.RouteTemplate = routePrefix + "/{documentName}/swagger.json");

        return builder.UseSwaggerUI(swaggerUiOptions =>
        {
            swaggerUiOptions.SwaggerEndpoint($"/{routePrefix}/{options.Name}/swagger.json", options.Title);
            swaggerUiOptions.RoutePrefix = routePrefix;
        });
    }
    
    #endregion
    
    public static async Task<IApplicationBuilder> UseDatabaseAsync<TDbContext>(this IApplicationBuilder builder, CancellationToken cancellationToken) 
        where TDbContext : DbContext
    {
        var options = builder.ApplicationServices.GetRequiredService<IOptions<EfDatabaseOptions>>().Value;
        var serviceScopeFactory = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
        await using var serviceScope = serviceScopeFactory.CreateAsyncScope();
        await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();

        var seeders = serviceScope.ServiceProvider.GetServices<SeederBase>();

        if (options.MigratingEnabled)
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        if (!options.SeedingEnabled)
        {
            return builder;
        }
        
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(cancellationToken);
        }

        return builder;
    }
    
    
}