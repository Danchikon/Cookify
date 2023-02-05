using System.Linq.Expressions;
using Cookify.Domain.MealCategory;

namespace Cookify.Application.Expressions;

public static class RecipeCategoryExpressions
{
    public static Expression<Func<RecipeCategoryEntity, bool>> NameEquals(string? name)
    {
        return recipeCategory => name == null || recipeCategory.Name.ToLower() == name.ToLower();
    }
    
    public static Expression<Func<RecipeCategoryEntity, bool>> CreateByEquals(Guid? createdBy, bool checkNull = true)
    {
        return recipeCategory => (checkNull && createdBy == null) || recipeCategory.CreatedBy == createdBy;
    }
    
    public static Expression<Func<RecipeCategoryEntity, bool>> NameContains(string? name)
    {
        return recipeCategory => name == null || recipeCategory.Name.ToLower().Contains(name.ToLower());
    }
    
    public static Expression<Func<RecipeCategoryEntity, bool>> UkrainianNameEquals(string? name)
    {
        return recipeCategory => name == null || recipeCategory.UkrainianName.ToLower() == name.ToLower();
    }
    
    public static Expression<Func<RecipeCategoryEntity, bool>> UkrainianNameContains(string? name)
    {
        return recipeCategory => name == null || recipeCategory.UkrainianName.ToLower().Contains(name.ToLower());
    }
}