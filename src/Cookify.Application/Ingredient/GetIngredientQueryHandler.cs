using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Dtos.Ingredient;
using Cookify.Application.Services;
using Cookify.Domain.Ingredient;
using Cookify.Domain.MealCategory;

namespace Cookify.Application.Ingredient;

public record GetIngredientQueryHandler : IQueryHandler<GetIngredientQuery, IngredientDto>
{
    private readonly IIngredientsRepository _ingredientsRepository;

    public GetIngredientQueryHandler(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }
    
    public async Task<IngredientDto> Handle(GetIngredientQuery query, CancellationToken cancellationToken)
    {
        var ingredient = await _ingredientsRepository.FirstAsync<IngredientDto>(query.Id);

        return ingredient;
    }
}