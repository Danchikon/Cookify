namespace Cookify.Infrastructure.Options;

public record TheMealDbApiOptions
{
    public const string SectionName = "TheMealDbApi";

    public string Url { get; init; } = null!;
}