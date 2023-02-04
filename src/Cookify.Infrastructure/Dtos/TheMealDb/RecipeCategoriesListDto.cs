using System.Text.Json.Serialization;

namespace Cookify.Infrastructure.Dtos.TheMealDb;

public record RecipeCategoriesListDto
{
    [JsonPropertyName("categories")]
    public ICollection<RecipeCategoryDto> Categories { get; init; } = Array.Empty<RecipeCategoryDto>();
}