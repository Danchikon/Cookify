using Cookify.Domain.Common.Entities;
using Cookify.Domain.User;

namespace Cookify.Domain.Session;

public class SessionEntity : IEntity<Guid>
{
    public Guid Id { get; init; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string RefreshTokenHash { get; set; } = null!;
    public DateTimeOffset SessionExpirationTime { get; set; }
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public SessionEntity()
    {
        
    }
    public SessionEntity(string refreshTokenHash, Guid userId, DateTimeOffset sessionExpirationTime)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        RefreshTokenHash = refreshTokenHash;
        SessionExpirationTime = sessionExpirationTime;
        UserId = userId;
    }
}