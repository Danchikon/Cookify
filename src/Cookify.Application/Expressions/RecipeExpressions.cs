using System.Linq.Expressions;
using Cookify.Domain.IngredientRecipe;
using Cookify.Domain.MealCategory;
using Cookify.Domain.Recipe;

namespace Cookify.Application.Expressions;

public static class RecipeExpressions
{
    public static Expression<Func<RecipeEntity, object?>> Category()
    {
        return recipe => recipe.Category;
    }
    
    public static Expression<Func<RecipeEntity, object?>> Likes()
    {
        return recipe => recipe.Likes;
    }
    
    public static Expression<Func<RecipeEntity, bool>> CreateByEquals(Guid? createdBy, bool checkNull = true)
    {
        return recipe => (checkNull && createdBy == null) || recipe.CreatedBy == createdBy;
    }
    
    public static Expression<Func<RecipeEntity, object?>> IngredientRecipes()
    {
        return recipe => recipe.IngredientRecipes;
    }
    
    public static Expression<Func<RecipeEntity, bool>> IsPublicEquals(bool? isPublic)
    {
        return recipe => isPublic == null || recipe.IsPublic == isPublic;
    }
    
    public static Expression<Func<RecipeEntity, bool>> TitleEquals(string? name)
    {
        return recipe => name == null || recipe.Title.ToLower() == name.ToLower();
    }
    
    public static Expression<Func<RecipeEntity, bool>> TitleContains(string? name)
    {
        return recipe => name == null || recipe.Title.ToLower().Contains(name.ToLower());
    }
    
    public static Expression<Func<RecipeEntity, bool>> UkrainianTitleEquals(string? name)
    {
        return recipe => name == null || recipe.UkrainianTitle.ToLower() == name.ToLower();
    }
    
    public static Expression<Func<RecipeEntity, bool>> UkrainianTitleContains(string? name)
    {
        return recipe => name == null || recipe.UkrainianTitle.ToLower().Contains(name.ToLower());
    }
    
    public static Expression<Func<RecipeEntity, bool>> CategoryIdEquals(Guid? id)
    {
        return recipe => id == null || recipe.CategoryId == id;
    }
    
    public static Expression<Func<RecipeEntity, bool>> IngredientsIdsIntersects(ICollection<Guid>? ids)
    {
        return recipe => ids == null || recipe.IngredientRecipes.Count(ingredientRecipe => ids.Contains(ingredientRecipe.IngredientId)) == ids.Count;
    }
}