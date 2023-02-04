namespace Cookify.Domain.Common.Entities;

public abstract class BaseEntity : IUserTrackableEntity
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? CreatedBy { get; init; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsActive { get; set; }

    protected BaseEntity(Guid? createdBy = null)
    {
        Id = Guid.NewGuid();
        CreatedBy = createdBy;
        CreatedAt = DateTimeOffset.UtcNow;
        IsActive = true;
    }
}