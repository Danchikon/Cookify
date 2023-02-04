using Cookify.Domain.Ingredient;
using Cookify.Domain.Recipe;

namespace Cookify.Domain.IngredientRecipe;

public class IngredientRecipeEntity 
{
    public Guid IngredientId { get; set; }
    public IngredientEntity Ingredient { get; set; } = null!;
    public Guid RecipeId { get; set; }
    public RecipeEntity Recipe { get; set; } = null!;
    
    public string Measure { get; set; } = null!;
    public string UkrainianMeasure { get; set; } = null!;

    public IngredientRecipeEntity()
    {
        
    }
    
    public IngredientRecipeEntity(
        Guid ingredientId,
        Guid recipeId,
        string measure,
        string ukrainianMeasure
        )
    {
        RecipeId = recipeId;
        IngredientId = ingredientId;
        Measure = measure;
        UkrainianMeasure = ukrainianMeasure;
    }

    public IngredientRecipeEntity(IngredientEntity ingredient, RecipeEntity recipe)
    {
        Ingredient = ingredient;
        IngredientId = ingredient.Id;
        Recipe = recipe;
        RecipeId = recipe.Id;
    }
}