namespace Cookify.Infrastructure.Options;

public sealed record GoogleImageSearcherOptions
{
    public const string SectionName = "GoogleImageSearcher";

    public string Url { get; init; } = null!;
    public int MaximumConcurrency { get; init; }
}