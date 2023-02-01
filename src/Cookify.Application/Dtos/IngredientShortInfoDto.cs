using Cookify.Domain.Ingredient.Enums;

namespace Cookify.Application.Dtos;

public record IngredientShortInfoDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public IngredientType Type { get; init; }
}