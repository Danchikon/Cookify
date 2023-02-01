using Cookify.Domain.Common.Entities;
using Cookify.Domain.Favorite;
using Cookify.Domain.Like;
using Cookify.Domain.Recipe;

namespace Cookify.Domain.User;

public class UserEntity : IEntity<Guid>
{
    public Guid Id { get; init; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public ICollection<FavoriteEntity> Favorites { get; set; } = Array.Empty<FavoriteEntity>();
    public ICollection<LikeEntity> Likes { get; set; } = Array.Empty<LikeEntity>();
    public ICollection<RecipeEntity> Recipes { get; set; } = Array.Empty<RecipeEntity>();

    public UserEntity()
    {
        
    }
    
    private UserEntity(string username, string passwordHash)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        Username = username;
        PasswordHash = passwordHash;
    }
}