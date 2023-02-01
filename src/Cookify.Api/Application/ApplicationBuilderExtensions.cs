using System.Net;
using Cookify.Api.Helpers;
using Cookify.Api.Middlewares;
using Cookify.Api.Options;
using Microsoft.Extensions.Options;

namespace Cookify.Api.Application;

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

        builder.UseStaticFiles()
            .UseSwagger(swaggerOptions => swaggerOptions.RouteTemplate = routePrefix + "/{documentName}/swagger.json");

        return builder.UseSwaggerUI(swaggerUiOptions =>
        {
            swaggerUiOptions.SwaggerEndpoint($"/{routePrefix}/{options.Name}/swagger.json", options.Title);
            swaggerUiOptions.RoutePrefix = routePrefix;
        });
    }
    
    #endregion
}