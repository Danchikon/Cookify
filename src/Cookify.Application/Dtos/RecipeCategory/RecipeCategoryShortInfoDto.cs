namespace Cookify.Application.Dtos.RecipeCategory;

public record RecipeCategoryShortInfoDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string UkrainianName { get; init; } = null!;
    public string? ImageLink { get; init; }
}