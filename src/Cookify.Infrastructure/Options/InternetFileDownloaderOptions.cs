namespace Cookify.Infrastructure.Options;

public sealed record InternetFileDownloaderOptions
{
    public const string SectionName = "InternetFileDownloader";
    
    public int MaximumConcurrency { get; init; }
    public int TimeoutInSeconds { get; init; }
}