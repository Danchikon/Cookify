using System.Linq.Expressions;
using Cookify.Domain.Ingredient;
using Cookify.Domain.MealCategory;

namespace Cookify.Application.Expressions;

public static class IngredientExpressions
{
    public static Expression<Func<IngredientEntity, bool>> NameEquals(string? name)
    {
        return mealCategory => name == null || mealCategory.Name.ToLower() == name.ToLower();
    }
    
    public static Expression<Func<IngredientEntity, bool>> NameContains(string? name)
    {
        return mealCategory => name == null || mealCategory.Name.ToLower().Contains(name.ToLower());
    }
    
    public static Expression<Func<IngredientEntity, bool>> UkrainianNameEquals(string? name)
    {
        return mealCategory => name == null || mealCategory.UkrainianName.ToLower() == name.ToLower();
    }
    
    public static Expression<Func<IngredientEntity, bool>> UkrainianNameContains(string? name)
    {
        return mealCategory => name == null || mealCategory.UkrainianName.ToLower().Contains(name.ToLower());
    }
}