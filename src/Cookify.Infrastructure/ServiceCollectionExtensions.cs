using Cookify.Application.Services;
using Cookify.Infrastructure.Options;
using Cookify.Infrastructure.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cookify.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICurrentUserService, HttpCurrentUserService>();
        services.AddGoogleFileStorage();

        return services;
    }

    public static IServiceCollection AddGoogleFileStorage(this IServiceCollection services)
    {
        GoogleStorageOptions options = new();
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(GoogleStorageOptions.SectionName);
            services.Configure<GoogleStorageOptions>(section);
            section.Bind(options);
        }

        services.AddScoped(_ => StorageClient.Create(GoogleCredential.FromFile(options.CredentialFileJson)));
        services.AddScoped<IFileStorageService, GoogleFileStorageService>();

        return services;
    }
}