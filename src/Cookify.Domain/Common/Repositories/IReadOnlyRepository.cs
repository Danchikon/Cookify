using System.Linq.Expressions;
using Cookify.Domain.Common.Pagination;

namespace Cookify.Domain.Common.Repositories;

public interface IReadOnlyRepository<TEntity>
{
    #region FirstAsync

    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? expression = null, Expression<Func<TEntity, object?>>? include = null);
    Task<TEntity> FirstAsync(Guid id, Expression<Func<TEntity, object?>>? include = null);
    Task<TModel> FirstAsync<TModel>(Expression<Func<TEntity, bool>>? expression = null, ICollection<Expression<Func<TEntity, object?>>>? includes = null);
    Task<TModel> FirstAsync<TModel>(Guid id, ICollection<Expression<Func<TEntity, object?>>>? includes = null);

    #endregion
    
    #region FirstOrDefaultAsync

    Task<TModel?> FirstOrDefaultAsync<TModel>(Expression<Func<TEntity, bool>>? expression = null, ICollection<Expression<Func<TEntity, object?>>>? includes = null);
    Task<TModel?> FirstOrDefaultAsync<TModel>(Guid id, ICollection<Expression<Func<TEntity, object?>>>? includes = null);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null, Expression<Func<TEntity, object?>>? include = null);
    Task<TEntity?> FirstOrDefaultAsync(Guid id, Expression<Func<TEntity, object?>>? include = null);
    
    #endregion

    #region WhereAsync

    Task<List<TModel>> WhereAsync<TModel>(params Expression<Func<TEntity, bool>>[]? expressions);
    Task<List<TEntity>> WhereAsync(ICollection<Expression<Func<TEntity, bool>>>? expressions = null, ICollection<Expression<Func<TEntity, object?>>>? includes = null);
    
    #endregion
        
    #region PaginateAsync

    Task<IPaginatedList<TModel>> PaginateAsync<TModel>(
        uint page,
        uint pageSize,
        uint offset = 0,
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        ICollection<Expression<Func<TEntity, bool>>>? expressions = null
    );

    #endregion
    
    #region AnyAsync
    
    Task<bool> AnyAsync(Guid id);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null);
    
    #endregion
    
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? expression = null);
}