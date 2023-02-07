using System.Linq.Expressions;
using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Recipe;

namespace Cookify.Application.Recipe;

public record GetRecipeShortInfosQueryHandler : IQueryHandler<GetRecipeShortInfosQuery, IPaginatedList<RecipeShortInfoDto>>
{
    private readonly IRecipesRepository _recipesRepository;
    private readonly Lazy<ICurrentUserService> _currentUserService;

    public GetRecipeShortInfosQueryHandler(
        IRecipesRepository recipesRepository,
        Lazy<ICurrentUserService> currentUserService
        )
    {
        _recipesRepository = recipesRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<IPaginatedList<RecipeShortInfoDto>> Handle(GetRecipeShortInfosQuery query, CancellationToken cancellationToken)
    {
        Guid? userId = null;
        
        if (query.IsPublicEquals is null or false)
        {
            userId = _currentUserService.Value.GetUserId();
        }
        
        var recipesPaginatedList = await _recipesRepository.PaginateAsync<RecipeShortInfoDto>(
            query.Pagination.Page,
            query.Pagination.PageSize,
            query.Pagination.Offset,
            expressions: new []
            {
                RecipeExpressions.CategoryIdEquals(query.CategoryIdEquals),
                RecipeExpressions.IsPublicEquals(query.IsPublicEquals),
                RecipeExpressions.CreateByEqualsNullOr(userId),
                RecipeExpressions.IngredientsIdsIntersects(query.IngredientsIdsIntersects),
                RecipeExpressions.UkrainianTitleContains(query.UkrainianTitleContains),
                RecipeExpressions.TitleContains(query.TitleContains),
                RecipeExpressions.UkrainianTitleEquals(query.UkrainianTitleEquals),
                RecipeExpressions.TitleEquals(query.TitleEquals)
            },
            includes: new []
            {
                RecipeExpressions.Likes()
            }, cancellationToken: cancellationToken);

        return recipesPaginatedList;
    }
}