using Cookify.Domain.Ingredient;
using Cookify.Domain.Recipe;

namespace Cookify.Domain.IngredientRecipe;

public class IngredientRecipeEntity 
{
    public Guid IngredientId { get; set; }
    public IngredientEntity Ingredient { get; set; }
    public Guid RecipeId { get; set; }
    public RecipeEntity Recipe { get; set; }

    public IngredientRecipeEntity(IngredientEntity ingredient, RecipeEntity recipe)
    {
        Ingredient = ingredient;
        IngredientId = ingredient.Id;
        Recipe = recipe;
        RecipeId = recipe.Id;
    }
}