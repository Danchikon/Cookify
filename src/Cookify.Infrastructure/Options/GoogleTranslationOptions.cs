namespace Cookify.Infrastructure.Options;

public sealed record GoogleTranslationOptions
{
    public const string SectionName = "GoogleTranslation";

    public string ApiKey { get; init; } = null!;
}