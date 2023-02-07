namespace Cookify.Infrastructure.Options;

public sealed record TheMealDbApiOptions
{
    public const string SectionName = "TheMealDbApi";

    public string Url { get; init; } = null!;
    public int TimeoutInSeconds { get; init; } 
}