using System.Text.Json.Serialization;

namespace Cookify.Infrastructure.Dtos.TheMealDb;

public record RecipesListDto
{
    [JsonPropertyName("meals")]
    public ICollection<RecipeDto>? Recipes { get; init; } = Array.Empty<RecipeDto>();
}