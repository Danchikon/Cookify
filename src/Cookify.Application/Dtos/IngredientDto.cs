using Cookify.Domain.Ingredient.Enums;

namespace Cookify.Application.Dtos;

public record IngredientDto
{
    public Guid Id { get; init; }
    public Guid? CreatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Title { get; init; } = null!;
    public IngredientType Type { get; init; }
    public string Description { get; init; } = string.Empty;
}