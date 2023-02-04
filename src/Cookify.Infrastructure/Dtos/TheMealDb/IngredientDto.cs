using System.Text.Json.Serialization;

namespace Cookify.Infrastructure.Dtos.TheMealDb;

public record IngredientDto
{
    [JsonPropertyName("ingredientId")] 
    public string Id { get; init; } = null!;
    
    [JsonPropertyName("strIngredient")] 
    public string Name { get; init; } = null!;
    
    [JsonPropertyName("strDescription")] 
    public string? Description { get; init; } 
    
    [JsonPropertyName("strType")] 
    public string? Type { get; init; } 
}