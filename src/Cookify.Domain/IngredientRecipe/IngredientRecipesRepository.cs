namespace Cookify.Domain.IngredientRecipe;

public interface IIngredientRecipesRepository
{
    Task AddAsync(IngredientRecipeEntity ingredientUser, CancellationToken cancellationToken);
}