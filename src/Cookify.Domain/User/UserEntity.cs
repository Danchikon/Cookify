using Cookify.Domain.Common.Entities;
using Cookify.Domain.Favorite;
using Cookify.Domain.IngredientUser;
using Cookify.Domain.Like;
using Cookify.Domain.Recipe;
using Cookify.Domain.Session;

namespace Cookify.Domain.User;

public class UserEntity : IEntity<Guid>
{
    public Guid Id { get; init; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? AvatarImageLink { get; set; }
    
    public Guid? SessionId { get; set; }
    public SessionEntity? Session { get; set; }

    public ICollection<FavoriteEntity> Favorites { get; set; } = new List<FavoriteEntity>();
    public ICollection<LikeEntity> Likes { get; set; } = new List<LikeEntity>();
    public ICollection<RecipeEntity> Recipes { get; set; } = new List<RecipeEntity>();
    
    public ICollection<IngredientUserEntity> IngredientUsers { get; set; } = new List<IngredientUserEntity>();

    public UserEntity()
    {
        
    }
    
    public UserEntity(
        string username, 
        string email, 
        string passwordHash,
        SessionEntity? session = null
        )
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
        Session = session;
        SessionId = session?.Id;
    }
}