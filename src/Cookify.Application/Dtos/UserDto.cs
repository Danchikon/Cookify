namespace Cookify.Application.Dtos;

public record UserDto
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Username { get; init; } = null!;
    public ICollection<RecipeDto> FavoriteRecipes { get; init; } = Array.Empty<RecipeDto>();
    public ICollection<RecipeDto> LikedRecipes { get; init; } = Array.Empty<RecipeDto>();
}