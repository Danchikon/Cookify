using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Ingredient;

namespace Cookify.Application.Ingredient;

public record GetIngredientShortInfosListQuery : QueryBase<IList<IngredientShortInfoDto>>
{
    public string? NameContains { get; set; }
    public string? NameEquals { get; set; }
    public string? UkrainianNameContains { get; set; }
    public string? UkrainianNameEquals { get; set; }
}