namespace Cookify.Infrastructure.Options;

public sealed record AuthenticationOptions
{
    public const string SectionName = "Authentication";
    
    public string? Issuer { get; init; } 
    public string? Audience { get; init; } 
    public string? SecretKey { get; init; } 
    public uint Lifetime { get; init; }
}