using System.Linq.Expressions;
using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Recipe;

namespace Cookify.Application.Recipe;

public record GetRandomRecipeShortInfosQueryHandler : IQueryHandler<GetRandomRecipeShortInfosQuery, IPaginatedList<RecipeShortInfoDto>>
{
    private readonly IRecipesRepository _recipesRepository;
    private readonly Lazy<ICurrentUserService> _currentUserService;

    public GetRandomRecipeShortInfosQueryHandler(
        IRecipesRepository recipesRepository,
        Lazy<ICurrentUserService> currentUserService
        )
    {
        _recipesRepository = recipesRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<IPaginatedList<RecipeShortInfoDto>> Handle(GetRandomRecipeShortInfosQuery query, CancellationToken cancellationToken)
    {
        Guid? userId = null;
        
        if (query.IsPublicEquals is null or false)
        {
            userId = _currentUserService.Value.GetUserId();
        }
        
        var random = new Random();

        var totalCount = await _recipesRepository.CountAsync(new [] {
            RecipeExpressions.CategoryIdEquals(query.CategoryIdEquals),
            RecipeExpressions.IsPublicEquals(query.IsPublicEquals),
            RecipeExpressions.CreateByEquals(userId)
        });
        
        var totalPages = totalCount / query.PageSize;
        
        var page = (uint)random.Next(1, (int)totalPages);
        
        var recipesPaginatedList = await _recipesRepository.PaginateAsync<RecipeShortInfoDto>(
            page, 
            query.PageSize, 
            expressions: new []
            {
                RecipeExpressions.CategoryIdEquals(query.CategoryIdEquals),
                RecipeExpressions.IsPublicEquals(query.IsPublicEquals),
                RecipeExpressions.CreateByEquals(userId),
            },
            includes: new []
            {
                RecipeExpressions.Likes()
            }
        );

        return recipesPaginatedList;
    }
}