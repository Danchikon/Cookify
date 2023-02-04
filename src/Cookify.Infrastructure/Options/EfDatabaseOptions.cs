namespace Cookify.Infrastructure.Options;

public sealed record EfDatabaseOptions
{
    public const string SectionName = "EfDatabase";
    
    public bool MigratingEnabled { get; init; } 
    public bool SeedingEnabled { get; init; } 
}