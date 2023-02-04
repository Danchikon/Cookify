using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Domain.RecipeCategory;

namespace Cookify.Application.RecipeCategory;

public record GetRecipeCategoryQueryHandler : IQueryHandler<GetRecipeCategoryQuery, RecipeCategoryDto>
{
    private readonly IRecipeCategoriesRepository _recipeCategoriesRepository;

    public GetRecipeCategoryQueryHandler(IRecipeCategoriesRepository recipeCategoriesRepository)
    {
        _recipeCategoriesRepository = recipeCategoriesRepository;
    }
    
    public async Task<RecipeCategoryDto> Handle(GetRecipeCategoryQuery query, CancellationToken cancellationToken)
    {
        var recipeCategory = await _recipeCategoriesRepository.FirstAsync<RecipeCategoryDto>(query.Id);

        return recipeCategory;
    }
}