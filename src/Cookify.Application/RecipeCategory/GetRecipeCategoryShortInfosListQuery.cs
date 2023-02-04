using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.RecipeCategory;

namespace Cookify.Application.RecipeCategory;

public record GetRecipeCategoryShortInfosListQuery : QueryBase<IList<RecipeCategoryShortInfoDto>>
{
    public string? NameContains { get; set; }
    public string? NameEquals { get; set; }
    public string? UkrainianNameContains { get; set; }
    public string? UkrainianNameEquals { get; set; }
}