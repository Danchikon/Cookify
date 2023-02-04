using Cookify.Application.Dtos.IngredientRecipe;
using Cookify.Application.Dtos.RecipeCategory;

namespace Cookify.Application.Dtos.Recipe;

public record RecipeShortInfoDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string UkrainianTitle { get; init; } = null!;
    public string? ImageLink { get; init; }
    public int LikesCount { get; init; }
    public RecipeCategoryShortInfoDto Category { get; init; } = null!;
    public ICollection<IngredientRecipeShortInfoDto> Ingredients { get; init; } = Array.Empty<IngredientRecipeShortInfoDto>();
}