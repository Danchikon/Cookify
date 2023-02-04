using System.Text.Json.Serialization;

namespace Cookify.Infrastructure.Dtos.TheMealDb;

public record IngredientsListDto
{
    [JsonPropertyName("meals")]
    public ICollection<IngredientDto> Ingredients { get; init; } = Array.Empty<IngredientDto>();
}