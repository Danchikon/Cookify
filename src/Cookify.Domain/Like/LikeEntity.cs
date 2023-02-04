using Cookify.Domain.Common.Entities;
using Cookify.Domain.Recipe;
using Cookify.Domain.User;

namespace Cookify.Domain.Like;

public class LikeEntity : BaseEntity
{
    public Guid RecipeId { get; set; }
    public RecipeEntity Recipe { get; set; } = null!;
    public UserEntity User { get; set; } = null!;

    public LikeEntity()
    {
        
    }
    
    public LikeEntity(RecipeEntity recipe, UserEntity user)
    {
        Recipe = recipe;
        RecipeId = recipe.Id;
        User = user;
        CreatedBy = user.Id;
    }
}