using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Application.RecipeCategory;
using Cookify.Application.Services;
using Cookify.Domain.MealCategory;
using Cookify.Domain.RecipeCategory;

namespace Cookify.Application.MealCategory;

public record GetRecipeCategoryShortInfoQueryHandler : IQueryHandler<GetRecipeCategoryShortInfoQuery, RecipeCategoryShortInfoDto>
{
    private readonly IRecipeCategoriesRepository _recipeCategoriesRepository;

    public GetRecipeCategoryShortInfoQueryHandler(IRecipeCategoriesRepository recipeCategoriesRepository)
    {
        _recipeCategoriesRepository = recipeCategoriesRepository;
    }
    
    public async Task<RecipeCategoryShortInfoDto> Handle(GetRecipeCategoryShortInfoQuery query, CancellationToken cancellationToken)
    {
        var recipeCategory = await _recipeCategoriesRepository.FirstAsync<RecipeCategoryShortInfoDto>(query.Id);

        return recipeCategory;
    }
}