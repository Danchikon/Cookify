using System.Linq.Expressions;
using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Expressions;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Recipe;

namespace Cookify.Application.Recipe;

public record GetRandomRecipeShortInfosQueryHandler : IQueryHandler<GetRandomRecipeShortInfosQuery, IPaginatedList<RecipeShortInfoDto>>
{
    private readonly IRecipesRepository _recipesRepository;

    public GetRandomRecipeShortInfosQueryHandler(IRecipesRepository recipesRepository)
    {
        _recipesRepository = recipesRepository;
    }
    
    public async Task<IPaginatedList<RecipeShortInfoDto>> Handle(GetRandomRecipeShortInfosQuery query, CancellationToken cancellationToken)
    {
        var random = new Random();

        var totalCount = await _recipesRepository.CountAsync(RecipeExpressions.CategoryIdEquals(query.CategoryIdEquals));
        var totalPages = totalCount / query.PageSize;
        
        var page = (uint)random.Next(1, (int)totalPages);
        
        var recipesPaginatedList = await _recipesRepository.PaginateAsync<RecipeShortInfoDto>(
            page, 
            query.PageSize, 
            expressions: new []
            {
                RecipeExpressions.CategoryIdEquals(query.CategoryIdEquals)
            },
            includes: new []
            {
                RecipeExpressions.Likes()
            }
        );

        return recipesPaginatedList;
    }
}