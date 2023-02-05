using System.Linq.Expressions;
using Cookify.Domain.Favorite;

namespace Cookify.Application.Expressions;

public static class FavoriteExpressions
{
    public static Expression<Func<FavoriteEntity, bool>> RecipeIdAndCreatedByEquals(Guid recipeId, Guid createdBy)
    {
        return favorite => favorite.CreatedBy == createdBy && favorite.RecipeId == recipeId;
    }
}