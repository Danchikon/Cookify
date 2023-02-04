using Cookify.Domain.Common.Entities;

namespace Cookify.Domain.Session;

public class SessionEntity : IEntity<Guid>
{
    public Guid Id { get; init; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string RefreshToken { get; set; } = null!;
    public DateTime SessionExpirationTime { get; set; }

    private SessionEntity()
    {
        
    }
    public SessionEntity(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}