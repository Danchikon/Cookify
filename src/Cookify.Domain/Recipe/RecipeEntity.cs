using Cookify.Domain.Common.Entities;
using Cookify.Domain.Favorite;
using Cookify.Domain.Ingredient;
using Cookify.Domain.Like;
using Cookify.Domain.User;

namespace Cookify.Domain.Recipe;

public class RecipeEntity : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Instruction { get; set; }
    
    public UserEntity? User { get; set; }
    
    public ICollection<FavoriteEntity> Favorites { get; set; } = Array.Empty<FavoriteEntity>();
    public ICollection<LikeEntity> Likes { get; set; } = Array.Empty<LikeEntity>();
    public ICollection<IngredientEntity> Ingredients { get; set; } = Array.Empty<IngredientEntity>();

    public RecipeEntity(
        string title, 
        string description,
        string instruction,
        Guid? createdBy
        ) : base(createdBy)
    {
        Title = title;
        Description = description;
        Instruction = instruction;
    }
}