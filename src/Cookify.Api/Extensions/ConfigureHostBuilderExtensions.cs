using Serilog;

namespace Cookify.Api.Extensions;

public static class ConfigureHostBuilderExtensions
{
    #region Serilog
    
    public static ConfigureHostBuilder ConfigureSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, serviceProvider, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            loggerConfiguration.ReadFrom.Services(serviceProvider);
            loggerConfiguration.Enrich.FromLogContext();
            loggerConfiguration.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}][{Level:u3}][{SourceContext}] {Message:lj}{Exception}{NewLine}");
        });

        return host;
    }
    
    #endregion
}