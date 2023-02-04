using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Pagination;
using Cookify.Application.Dtos.Ingredient;

namespace Cookify.Application.Ingredient;

public record GetIngredientShortInfosQuery(PaginationOptions? Pagination) 
    : PaginationQuery<IngredientShortInfoDto>(Pagination ?? new PaginationOptions())
{
    public string? NameContains { get; set; }
    public string? NameEquals { get; set; }
    public string? UkrainianNameContains { get; set; }
    public string? UkrainianNameEquals { get; set; }
}