using Cookify.Application.Dtos.Ingredient;
using Cookify.Application.Dtos.IngredientUser;
using Cookify.Application.Dtos.Recipe;

namespace Cookify.Application.Dtos.User;

public record UserDto
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Email { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string? AvatarImageLink { get; init; }
    public ICollection<RecipeShortInfoDto> Recipes { get; init; } = Array.Empty<RecipeShortInfoDto>();
    public ICollection<RecipeShortInfoDto> FavoriteRecipes { get; init; } = Array.Empty<RecipeShortInfoDto>();
    public ICollection<RecipeShortInfoDto> LikedRecipes { get; init; } = Array.Empty<RecipeShortInfoDto>();
    public ICollection<IngredientUserShortInfoDto> AvailableIngredients { get; init; } = Array.Empty<IngredientUserShortInfoDto>();
}