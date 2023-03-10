namespace Cookify.Domain.Common.Entities;

public interface IEntity<TId>
{
    TId Id { get; init; }
    bool IsActive  { get; set; }
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}