using Cookify.Domain.Common.Entities;

namespace Cookify.Domain.Common.Repositories;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : IEntity<Guid>
{
    Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    ValueTask PartiallyUpdateAsync(Guid id, PartialEntity<TEntity> partialEntity, CancellationToken cancellationToken = default);
    ValueTask UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    ValueTask RemoveAsync(Guid id, bool softRemove = true, CancellationToken cancellationToken = default);
    
}