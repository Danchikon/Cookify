using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.RecipeCategory;
using Cookify.Domain.RecipeCategory;

namespace Cookify.Application.RecipeCategory;

public record GetRecipeCategoryShortInfoQueryHandler : IQueryHandler<GetRecipeCategoryShortInfoQuery, RecipeCategoryShortInfoDto>
{
    private readonly IRecipeCategoriesRepository _recipeCategoriesRepository;

    public GetRecipeCategoryShortInfoQueryHandler(IRecipeCategoriesRepository recipeCategoriesRepository)
    {
        _recipeCategoriesRepository = recipeCategoriesRepository;
    }
    
    public async Task<RecipeCategoryShortInfoDto> Handle(GetRecipeCategoryShortInfoQuery query, CancellationToken cancellationToken)
    {
        var recipeCategory = await _recipeCategoriesRepository.FirstAsync<RecipeCategoryShortInfoDto>(query.Id, cancellationToken: cancellationToken);

        return recipeCategory;
    }
}