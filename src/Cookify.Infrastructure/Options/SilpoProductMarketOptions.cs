namespace Cookify.Infrastructure.Options;

public sealed record SilpoProductMarketOptions
{
    public const string SectionName = "SilpoProductMarket";

    public string SiteUrl { get; init; } = null!;
    public string ApiUrl { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
}