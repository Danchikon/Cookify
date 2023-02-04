using System.Linq.Expressions;
using Cookify.Domain.MealCategory;
using Cookify.Domain.ProductMarket;

namespace Cookify.Application.Expressions;

public static class ProductMarketExpressions
{
    public static Expression<Func<ProductMarketEntity, bool>> NameEquals(string? name)
    {
        return mealCategory => name == null || mealCategory.Name.ToLower() == name.ToLower();
    }
    
    public static Expression<Func<ProductMarketEntity, bool>> NameContains(string? name)
    {
        return mealCategory => name == null || mealCategory.Name.ToLower().Contains(name.ToLower());
    }
}