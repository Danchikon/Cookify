using System.Text.Json.Serialization;

namespace Cookify.Infrastructure.Dtos.TheMealDb;

public record RecipeCategoryDto
{
    [JsonPropertyName("idCategory")]
    public string Id { get; init; } = null!;
    
    [JsonPropertyName("strCategory")]
    public string Name { get; init; } = null!;
    
    [JsonPropertyName("strCategoryThumb")]
    public string ImageLink { get; init; } = null!;
    
    [JsonPropertyName("strCategoryDescription")]
    public string? Description { get; init; } 
}