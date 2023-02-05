namespace Cookify.Application.Dtos.IngredientUser;

public record IngredientUserShortInfoDto
{
    public Guid IngredientId { get; init; }
    public string Name { get; init; } = null!;
    public string UkrainianName { get; init; } = null!;
    public string? ImageLink { get; set; }
    public string UkrainianMeasure { get; set; } = null!;
}