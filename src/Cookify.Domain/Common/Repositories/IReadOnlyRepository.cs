using System.Linq.Expressions;
using Cookify.Domain.Common.Pagination;

namespace Cookify.Domain.Common.Repositories;

public interface IReadOnlyRepository<TEntity>
{
    #region FirstAsync

    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? expression = null);
    Task<TEntity> FirstAsync(Guid id);
    Task<TModel> FirstAsync<TModel>(Expression<Func<TEntity, bool>>? expression = null);
    Task<TModel> FirstAsync<TModel>(Guid id);

    #endregion
    
    #region FirstOrDefaultAsync

    Task<TModel?> FirstOrDefaultAsync<TModel>(Expression<Func<TEntity, bool>>? expression = null);
    Task<TModel?> FirstOrDefaultAsync<TModel>(Guid id);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null);
    Task<TEntity?> FirstOrDefaultAsync(Guid id);
    
    #endregion

    #region WhereAsync

    Task<List<TModel>> WhereAsync<TModel>(params Expression<Func<TEntity, bool>>[]? expressions);
    Task<List<TEntity>> WhereAsync(params Expression<Func<TEntity, bool>>[]? expressions);
    
    #endregion
        
    #region PaginateAsync

    Task<IPaginatedList<TModel>> PaginateAsync<TModel>(
        uint page,
        uint pageSize,
        uint offset = 0,
        ICollection<Expression<Func<TEntity, object>>>? includes = null,
        ICollection<Expression<Func<TEntity, bool>>>? expressions = null
    );

    #endregion
    
    #region AnyAsync
    
    Task<bool> AnyAsync(Guid id);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null);
    
    #endregion
}