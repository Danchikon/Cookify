namespace Cookify.Infrastructure.Options;

public sealed record SwaggerOptions
{
    public const string SectionName = "Swagger";
    
    public bool Enabled { get; init; }
    public string? Name { get; init; }
    public string? Title { get; init; }
    public string? Version { get; init; }
    public string? RoutePrefix { get; init; }
    public bool IncludeSecurity { get; init; }
}
