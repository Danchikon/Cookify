namespace Cookify.Infrastructure.Common.Helpers;

public static class AspNetCoreEnvironment
{
    public static readonly string? Name = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    public static bool IsProduction => Name is "Production";
}