namespace Cookify.Infrastructure.Options;

public sealed record AuthenticationOptions
{
    public const string SectionName = "Authentication";
    
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public uint Lifetime { get; init; }
}