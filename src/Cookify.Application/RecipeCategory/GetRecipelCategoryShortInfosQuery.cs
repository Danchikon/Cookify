using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Pagination;
using Cookify.Application.Dtos.RecipeCategory;

namespace Cookify.Application.RecipeCategory;

public record GetRecipeCategoryShortInfosQuery(PaginationOptions? Pagination) 
    : PaginationQuery<RecipeCategoryShortInfoDto>(Pagination ?? new PaginationOptions())
{
    public string? NameContains { get; set; }
    public string? NameEquals { get; set; }
    public string? UkrainianNameContains { get; set; }
    public string? UkrainianNameEquals { get; set; }
}