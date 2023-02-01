namespace Cookify.Application.Dtos;

public record RecipeDto
{
    public Guid Id { get; init; }
    public Guid? CreatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = string.Empty;
    public string Instruction { get; init; } = string.Empty;
    public uint LikesCount { get; init; }
    public ICollection<IngredientDto> Ingredients { get; init; } = Array.Empty<IngredientDto>();
}