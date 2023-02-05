using System.Linq.Expressions;
using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Expressions;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Recipe;

namespace Cookify.Application.Recipe;

public record GetRecipeShortInfosQueryHandler : IQueryHandler<GetRecipeShortInfosQuery, IPaginatedList<RecipeShortInfoDto>>
{
    private readonly IRecipesRepository _recipesRepository;

    public GetRecipeShortInfosQueryHandler(IRecipesRepository recipesRepository)
    {
        _recipesRepository = recipesRepository;
    }
    
    public async Task<IPaginatedList<RecipeShortInfoDto>> Handle(GetRecipeShortInfosQuery query, CancellationToken cancellationToken)
    {
        var recipesPaginatedList = await _recipesRepository.PaginateAsync<RecipeShortInfoDto>(
            query.Pagination.Page,
            query.Pagination.PageSize,
            query.Pagination.Offset,
            query.TitleEquals,
            query.TitleContains,
            query.UkrainianTitleEquals,
            query.UkrainianTitleContains,
            query.CategoryIdEquals,
            query.IngredientsIdsIntersects
        );

        return recipesPaginatedList;
    }
}