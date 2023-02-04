namespace Cookify.Infrastructure.Options;

public class GoogleTranslationOptions
{
    public const string SectionName = "GoogleTranslation";

    public string ApiKey { get; init; } = null!;
}