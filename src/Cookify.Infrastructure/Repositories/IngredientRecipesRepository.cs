using Cookify.Domain.Common.Exceptions;
using Cookify.Domain.Exceptions;
using Cookify.Domain.IngredientRecipe;
using Cookify.Domain.IngredientUser;
using Cookify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Infrastructure.Repositories;

public class IngredientRecipesRepository : IIngredientRecipesRepository
{
    private readonly CookifyDbContext _dbContext;
    
    public IngredientRecipesRepository(CookifyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(IngredientRecipeEntity ingredientRecipe, CancellationToken cancellationToken)
    {
        await _dbContext.IngredientRecipes.AddAsync(ingredientRecipe, cancellationToken);
    }
}