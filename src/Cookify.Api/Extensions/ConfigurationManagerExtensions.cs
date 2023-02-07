using Cookify.Infrastructure.Common.Helpers;

namespace Cookify.Api.Extensions;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddJsonFiles(this ConfigurationManager manager)
    {
        const string appSettingsFileName = "appsettings";
        const string appSettingsFileExtension = "json";

        manager.AddJsonFile($"{appSettingsFileName}.{appSettingsFileExtension}");
        manager.AddJsonFile($"{appSettingsFileName}.{AspNetCoreEnvironment.Name}.{appSettingsFileExtension}", true);
        manager.AddEnvironmentVariables();

        return manager;
    }
}