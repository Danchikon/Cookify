using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Ingredient;
using Cookify.Domain.Ingredient;
using Cookify.Domain.MealCategory;

namespace Cookify.Application.Ingredient;

public record GetIngredientShortInfoQueryHandler : IQueryHandler<GetIngredientShortInfoQuery, IngredientShortInfoDto>
{
    private readonly IIngredientsRepository _ingredientsRepository;

    public GetIngredientShortInfoQueryHandler(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }
    
    public async Task<IngredientShortInfoDto> Handle(GetIngredientShortInfoQuery query, CancellationToken cancellationToken)
    {
        var ingredient = await _ingredientsRepository.FirstAsync<IngredientShortInfoDto>(query.Id, cancellationToken: cancellationToken);

        return ingredient;
    }
}