using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using Cookify.Application.Common.Dtos;
using Cookify.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Cookify.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddApiControllers();
        services.AddSwagger();
        services.AddJwtBearerAuthentication();
        services.AddCompression();

        return services;
    }
    
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {

        services
            .AddControllers()
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

    #region Compression

    public static IServiceCollection AddCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });
        
        return services;
    }

    #endregion
    
    #region Authentication

    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services)
    {
        AuthenticationOptions options = new();
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(AuthenticationOptions.SectionName);
            services.Configure<AuthenticationOptions>(section);
            section.Bind(options);
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtBearerOptions => 
            {
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = options.Audience,
                    ValidIssuer = options.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)) 
                }; 
            });

        return services;
    }
    
    #endregion
    
    #region Swagger

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        SwaggerOptions options = new();
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(SwaggerOptions.SectionName);
            services.Configure<SwaggerOptions>(section, binderOptions => binderOptions.ErrorOnUnknownConfiguration = true);
            section.Bind(options);
        }
        
        if (!options.Enabled)
        {
            return services;
        }

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