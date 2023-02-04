namespace Cookify.Application.Dtos.IngredientRecipe;

public record IngredientRecipeShortInfoDto
{
    public Guid IngredientId { get; init; }
    public string Name { get; init; } = null!;
    public string UkrainianName { get; init; } = null!;
    public string? ImageLink { get; set; } 
    public string Measure { get; set; } = null!;
    public string UkrainianMeasure { get; set; } = null!;
}