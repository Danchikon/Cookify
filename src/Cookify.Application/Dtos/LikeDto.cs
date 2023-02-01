namespace Cookify.Application.Dtos;

public record LikeDto
{
    public Guid Id { get; init; }
    public Guid RecipeId { get; init; }
    public Guid? CreatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}