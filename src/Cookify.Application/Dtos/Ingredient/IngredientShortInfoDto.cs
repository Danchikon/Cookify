namespace Cookify.Application.Dtos.Ingredient;

public record IngredientShortInfoDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string UkrainianName { get; init; } = null!;
    public string? ImageLink { get; set; } 
}