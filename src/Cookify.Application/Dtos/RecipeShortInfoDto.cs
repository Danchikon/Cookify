namespace Cookify.Application.Dtos;

public record RecipeShortInfoDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public uint LikesCount { get; init; }
}