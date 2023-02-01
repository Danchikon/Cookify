namespace Cookify.Api.Application;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddJsonFiles(this ConfigurationManager manager)
    {
        const string appSettingsFileName = "appsettings";
        const string appSettingsFileExtension = "json";

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        manager.AddJsonFile($"{appSettingsFileName}.{appSettingsFileExtension}");
        manager.AddJsonFile($"{appSettingsFileName}.{environment}.{appSettingsFileExtension}", true);

        return manager;
    }
}