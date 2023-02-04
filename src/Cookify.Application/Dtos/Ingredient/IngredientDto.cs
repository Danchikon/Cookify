namespace Cookify.Application.Dtos.Ingredient;

public record IngredientDto
{
    public Guid Id { get; init; }
    public Guid? CreatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Name { get; init; } = null!;
    public string UkrainianName { get; init; } = null!;
    public string? Description { get; init; }
    public string? UkrainianDescription { get; init; }
    public string? ImageLink { get; set; } 
}