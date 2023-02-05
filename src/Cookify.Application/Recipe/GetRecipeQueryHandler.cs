using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Expressions;
using Cookify.Domain.Recipe;

namespace Cookify.Application.Recipe;

public record GetRecipeQueryHandler : IQueryHandler<GetRecipeQuery, RecipeDto>
{
    private readonly IRecipesRepository _recipesRepository;

    public GetRecipeQueryHandler(IRecipesRepository recipesRepository)
    {
        _recipesRepository = recipesRepository;
    }
    
    public async Task<RecipeDto> Handle(GetRecipeQuery query, CancellationToken cancellationToken)
    {
        var recipe = await _recipesRepository.FirstAsync<RecipeDto>(query.Id, includes: new []
        {
            RecipeExpressions.Likes()
        });

        return recipe;
    }
}