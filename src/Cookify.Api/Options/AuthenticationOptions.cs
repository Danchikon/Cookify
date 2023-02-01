namespace Cookify.Api.Options;

public sealed class AuthenticationOptions
{
    public const string SectionName = "Authentication";
    
    public string Authority { get; init; } = null!;
}