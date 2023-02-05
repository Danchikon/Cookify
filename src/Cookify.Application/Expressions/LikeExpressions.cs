using System.Linq.Expressions;
using Cookify.Domain.Favorite;
using Cookify.Domain.Like;

namespace Cookify.Application.Expressions;

public static class LikeExpressions
{
    public static Expression<Func<LikeEntity, bool>> RecipeIdAndCreatedByEquals(Guid recipeId, Guid createdBy)
    {
        return favorite => favorite.CreatedBy == createdBy && favorite.RecipeId == recipeId;
    }
}