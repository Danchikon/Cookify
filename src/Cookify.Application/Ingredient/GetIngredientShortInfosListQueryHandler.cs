using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Ingredient;
using Cookify.Application.Expressions;
using Cookify.Domain.Ingredient;

namespace Cookify.Application.Ingredient;

public record GetIngredientShortInfosListQueryHandler : IQueryHandler<GetIngredientShortInfosListQuery, IList<IngredientShortInfoDto>>
{
    private readonly IIngredientsRepository _ingredientsRepository;

    public GetIngredientShortInfosListQueryHandler(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }
    
    public async Task<IList<IngredientShortInfoDto>> Handle(GetIngredientShortInfosListQuery query, CancellationToken cancellationToken)
    {
        var mealCategoriesList = await _ingredientsRepository.WhereAsync<IngredientShortInfoDto>(
            IngredientExpressions.NameEquals(query.NameEquals), 
            IngredientExpressions.NameContains(query.NameContains),
            IngredientExpressions.UkrainianNameEquals(query.UkrainianNameEquals), 
            IngredientExpressions.UkrainianNameContains(query.UkrainianNameContains)
        );

        return mealCategoriesList;
    }
}