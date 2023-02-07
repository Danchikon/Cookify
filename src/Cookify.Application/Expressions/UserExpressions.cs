using System.Linq.Expressions;
using Cookify.Domain.Favorite;
using Cookify.Domain.Session;
using Cookify.Domain.User;

namespace Cookify.Application.Expressions;

public static class UserExpressions
{
    public static Expression<Func<UserEntity, bool>> UsernameEquals(string? username)
    {
        return user => username == null || user.Username.ToLower() == username.ToLower();
    }
    
    public static Expression<Func<UserEntity, bool>> EmailEquals(string? email)
    {
        return user => email == null || user.Email.ToLower() == email.ToLower();
    }
    
    public static Expression<Func<UserEntity, object?>> Session()
    {
        return user => user.Session;
    }
    
    public static Expression<Func<UserEntity, object?>> Favorites()
    {
        return user => user.Favorites;
    }
    
    public static Expression<Func<UserEntity, object?>> Recipes()
    {
        return user => user.Recipes;
    }
    
    public static Expression<Func<UserEntity, object?>> Likes()
    {
        return user => user.Likes;
    }
    
    public static Expression<Func<UserEntity, object?>> IngredientUsers()
    {
        return user => user.IngredientUsers;
    }
}