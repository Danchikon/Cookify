using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Application.Expressions;
using Cookify.Domain.RecipeCategory;

namespace Cookify.Application.RecipeCategory;

public record GetRecipeCategoryShortInfosListQueryHandler : IQueryHandler<GetRecipeCategoryShortInfosListQuery, IList<RecipeCategoryShortInfoDto>>
{
    private readonly IRecipeCategoriesRepository _recipeCategoriesRepository;

    public GetRecipeCategoryShortInfosListQueryHandler(IRecipeCategoriesRepository recipeCategoriesRepository)
    {
        _recipeCategoriesRepository = recipeCategoriesRepository;
    }
    
    public async Task<IList<RecipeCategoryShortInfoDto>> Handle(GetRecipeCategoryShortInfosListQuery query, CancellationToken cancellationToken)
    {
        var recipeCategoriesList = await _recipeCategoriesRepository.WhereAsync<RecipeCategoryShortInfoDto>(
            RecipeCategoryExpressions.NameEquals(query.NameEquals), 
            RecipeCategoryExpressions.NameContains(query.NameContains),
            RecipeCategoryExpressions.UkrainianNameEquals(query.UkrainianNameEquals), 
            RecipeCategoryExpressions.UkrainianNameContains(query.UkrainianNameContains)
        );

        return recipeCategoriesList;
    }
}