using Cookify.Application.Dtos.IngredientRecipe;
using Cookify.Application.Dtos.RecipeCategory;

namespace Cookify.Application.Dtos.Recipe;

public record RecipeShortInfoDto
{
    public Guid Id { get; init; }
    public Guid? CreatedBy { get; init; }
    public string Title { get; init; } = null!;
    public string UkrainianTitle { get; init; } = null!;
    public string? ImageLink { get; init; }
    public string? PdfLink { get; init; }
    public string? UkrainianPdfLink { get; init; }
    public bool IsPublic { get; init; }
    public int LikesCount { get; init; }
    public RecipeCategoryShortInfoDto Category { get; init; } = null!;
    public ICollection<IngredientRecipeShortInfoDto> Ingredients { get; init; } = Array.Empty<IngredientRecipeShortInfoDto>();
}