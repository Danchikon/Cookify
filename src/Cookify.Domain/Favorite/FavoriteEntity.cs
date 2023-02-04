using Cookify.Domain.Common.Entities;
using Cookify.Domain.Recipe;
using Cookify.Domain.User;

namespace Cookify.Domain.Favorite;

public class FavoriteEntity : BaseEntity
{
    public Guid RecipeId { get; set; }
    public RecipeEntity Recipe { get; set; } = null!;
    public UserEntity User { get; set; } = null!;

    public FavoriteEntity()
    {
        
    }
    public FavoriteEntity(RecipeEntity recipe, UserEntity user)
    {
        Recipe = recipe;
        RecipeId = recipe.Id;
        User = user;
        CreatedBy = user.Id;
    }
    
    public FavoriteEntity(Guid recipeId, Guid userId)
    {
        RecipeId = recipeId;
        CreatedBy = userId;
    }
}