using Cookify.Domain.Common.Entities;

namespace Cookify.Domain.Common.Repositories;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : IEntity<Guid>
{
    Task<Guid> AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    ValueTask PartiallyUpdateAsync(Guid id, PartialEntity<TEntity> partialEntity);
    ValueTask UpdateAsync(TEntity entity);
    ValueTask RemoveAsync(Guid id);
    
}