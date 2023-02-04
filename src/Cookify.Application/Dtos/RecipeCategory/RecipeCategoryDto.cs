namespace Cookify.Application.Dtos.RecipeCategory;

public record RecipeCategoryDto
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Name { get; init; } = null!;
    public string UkrainianName { get; init; } = null!;
    public string? Description { get; init; } 
    public string? UkrainianDescription { get; init; } 
    public string? ImageLink { get; init; } 
}