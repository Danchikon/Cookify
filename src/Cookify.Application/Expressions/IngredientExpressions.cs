using System.Linq.Expressions;
using Cookify.Domain.Ingredient;
using Cookify.Domain.MealCategory;

namespace Cookify.Application.Expressions;

public static class IngredientExpressions
{
    public static Expression<Func<IngredientEntity, bool>> NameEquals(string? name)
    {
        return ingredient => name == null || ingredient.Name.ToLower() == name.ToLower();
    }
    
    public static Expression<Func<IngredientEntity, bool>> CreateByEquals(Guid? createdBy, bool checkNull = true)
    {
        return ingredient => (checkNull && createdBy == null) || ingredient.CreatedBy == createdBy;
    }
    
    public static Expression<Func<IngredientEntity, bool>> NameContains(string? name)
    {
        return ingredient => name == null || ingredient.Name.ToLower().Contains(name.ToLower());
    }
    
    public static Expression<Func<IngredientEntity, bool>> UkrainianNameEquals(string? name)
    {
        return ingredient => name == null || ingredient.UkrainianName.ToLower() == name.ToLower();
    }
    
    public static Expression<Func<IngredientEntity, bool>> UkrainianNameContains(string? name)
    {
        return ingredient => name == null || ingredient.UkrainianName.ToLower().Contains(name.ToLower());
    }
}