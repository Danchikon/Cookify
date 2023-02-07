using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Ingredient;
using Cookify.Application.Expressions;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Ingredient;

namespace Cookify.Application.Ingredient;

public record GetIngredientShortInfosQueryHandler : IQueryHandler<GetIngredientShortInfosQuery, IPaginatedList<IngredientShortInfoDto>>
{
    private readonly IIngredientsRepository _ingredientsRepository;

    public GetIngredientShortInfosQueryHandler(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }
    
    public async Task<IPaginatedList<IngredientShortInfoDto>> Handle(GetIngredientShortInfosQuery query, CancellationToken cancellationToken)
    {
        var ingredientsPaginatedList = await _ingredientsRepository.PaginateAsync<IngredientShortInfoDto>(
            query.Pagination.Page,
            query.Pagination.PageSize,
            query.Pagination.Offset,
            expressions: new [] { 
                IngredientExpressions.NameEquals(query.NameEquals),
                IngredientExpressions.NameContains(query.NameContains),
                IngredientExpressions.UkrainianNameEquals(query.UkrainianNameEquals),
                IngredientExpressions.UkrainianNameContains(query.UkrainianNameContains)
            }, 
            cancellationToken: cancellationToken
            );

        return ingredientsPaginatedList;
    }
}