using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Application.Expressions;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.RecipeCategory;

namespace Cookify.Application.RecipeCategory;

public record GetRecipeCategoryShortInfosQueryHandler : IQueryHandler<GetRecipeCategoryShortInfosQuery, IPaginatedList<RecipeCategoryShortInfoDto>>
{
    private readonly IRecipeCategoriesRepository _recipeCategoriesRepository;

    public GetRecipeCategoryShortInfosQueryHandler(IRecipeCategoriesRepository recipeCategoriesRepository)
    {
        _recipeCategoriesRepository = recipeCategoriesRepository;
    }
    
    public async Task<IPaginatedList<RecipeCategoryShortInfoDto>> Handle(GetRecipeCategoryShortInfosQuery query, CancellationToken cancellationToken)
    {
        var recipeCategoriesPaginatedList = await _recipeCategoriesRepository.PaginateAsync<RecipeCategoryShortInfoDto>(
            query.Pagination.Page,
            query.Pagination.PageSize,
            query.Pagination.Offset,
            expressions: new [] { 
                RecipeCategoryExpressions.NameEquals(query.NameEquals),
                RecipeCategoryExpressions.NameContains(query.NameContains),
                RecipeCategoryExpressions.UkrainianNameEquals(query.UkrainianNameEquals),
                RecipeCategoryExpressions.UkrainianNameContains(query.UkrainianNameContains)
            }, 
            cancellationToken: cancellationToken
            );

        return recipeCategoriesPaginatedList;
    }
}