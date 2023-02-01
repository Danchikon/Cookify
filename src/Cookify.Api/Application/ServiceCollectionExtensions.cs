using System.Text.Json.Serialization;
using Cookify.Api.Options;
using Cookify.Application.Common.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Cookify.Api.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
     

        services.AddApiControllers();
        services.AddCustomSwagger();
        
        return services;
    }
    
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {

        services.AddControllers()
            .AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }).ConfigureApiBehaviorOptions(config =>
            {
                config.InvalidModelStateResponseFactory = context =>
                {
                    var errorMessages = new List<string>();
                    
                    foreach (var (_, value) in context.ModelState)
                    {
                        errorMessages.AddRange(value.Errors.Select(x => x.ErrorMessage));
                    }
            
                    return new BadRequestObjectResult(new ErrorDto
                    {
                        Title = "InvalidModelState",
                        Messages = errorMessages
                    });
                };
            });
        
        return services;
    }
    
    #region Swagger

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        SwaggerOptions options = new();
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(SwaggerOptions.SectionName);
            services.Configure<SwaggerOptions>(section);
            section.Bind(options);
        }
        
        if (!options.Enabled) return services;

        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(options.Name, new OpenApiInfo {Title = options.Title, Version = options.Version});
            c.EnableAnnotations();
            
            if (!options.IncludeSecurity)
            {
                return;
            }
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
    
    #endregion
}