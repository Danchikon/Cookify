using System.Linq.Expressions;
using Cookify.Domain.Common.Pagination;

namespace Cookify.Domain.Common.Repositories;

public interface IReadOnlyRepository<TEntity>
{
    #region FirstAsync

    Task<TEntity> FirstAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        Expression<Func<TEntity, object?>>? include = null,
        CancellationToken cancellationToken = default
        );
    Task<TEntity> FirstAsync(
        Guid id, 
        Expression<Func<TEntity, object?>>? include = null,
        CancellationToken cancellationToken = default
        );
    Task<TModel> FirstAsync<TModel>(
        Expression<Func<TEntity, bool>>? expression = null,
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        CancellationToken cancellationToken = default
        );
    Task<TModel> FirstAsync<TModel>(
        Guid id, 
        ICollection<Expression<Func<TEntity, object?>>>? includes = null, 
        CancellationToken cancellationToken = default
        );

    #endregion
    
    #region FirstOrDefaultAsync

    Task<TModel?> FirstOrDefaultAsync<TModel>(
        Expression<Func<TEntity, bool>>? expression = null,
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        CancellationToken cancellationToken = default
        );
    Task<TModel?> FirstOrDefaultAsync<TModel>(
        Guid id, 
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        CancellationToken cancellationToken = default
        );
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? expression = null, 
        Expression<Func<TEntity, object?>>? include = null, 
        CancellationToken cancellationToken = default
        );
    Task<TEntity?> FirstOrDefaultAsync(
        Guid id, 
        Expression<Func<TEntity, object?>>? include = null, 
        CancellationToken cancellationToken = default
        );
    
    #endregion

    #region WhereAsync

    Task<List<TModel>> WhereAsync<TModel>(
        ICollection<Expression<Func<TEntity, bool>>>? expressions = null, 
        CancellationToken cancellationToken = default
        );
    Task<List<TEntity>> WhereAsync(
        ICollection<Expression<Func<TEntity, bool>>>? expressions = null, 
        ICollection<Expression<Func<TEntity, object?>>>? includes = null, 
        CancellationToken cancellationToken = default
        );
    
    #endregion
        
    #region PaginateAsync

    Task<IPaginatedList<TModel>> PaginateAsync<TModel>(
        uint page,
        uint pageSize,
        uint offset = 0,
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        ICollection<Expression<Func<TEntity, bool>>>? expressions = null, 
        CancellationToken cancellationToken = default
    );

    #endregion
    
    #region AnyAsync
    
    Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default);
    
    #endregion
    
    Task<int> CountAsync(ICollection<Expression<Func<TEntity, bool>>>? expressions = null, CancellationToken cancellationToken = default);
}