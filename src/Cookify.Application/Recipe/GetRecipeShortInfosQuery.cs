using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Pagination;
using Cookify.Application.Dtos.Recipe;

namespace Cookify.Application.Recipe;

public record GetRecipeShortInfosQuery(PaginationOptions? Pagination) 
    : PaginationQuery<RecipeShortInfoDto>(Pagination ?? new PaginationOptions())
{
    public string? TitleContains { get; set; }
    public string? TitleEquals { get; set; }
    public string? UkrainianTitleContains { get; set; }
    public string? UkrainianTitleEquals { get; set; }
}