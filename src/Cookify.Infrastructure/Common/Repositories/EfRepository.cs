using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cookify.Domain.Common.Entities;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Common.Repositories;
using Cookify.Domain.Exceptions;
using Cookify.Infrastructure.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Infrastructure.Common.Repositories;

public class EfRepository<TEntity, TContext> : IRepository<TEntity> 
    where TEntity : class, IEntity<Guid>, new()
    where TContext : DbContext
{
    protected readonly TContext DbContext;
    protected readonly IMapper Mapper;

    public EfRepository(TContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        Mapper = mapper;
    }
    
    public virtual async Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        return result.Entity.Id;
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
    }

    public virtual ValueTask PartiallyUpdateAsync(Guid id, PartialEntity<TEntity> partialEntity, CancellationToken cancellationToken = default)
    {
        TEntity entity = new() { Id = id };
        
        DbContext.Set<TEntity>().Attach(entity);
        
        var references = DbContext.Entry(entity)
                .References
                .Select(x => x.Metadata.Name)
                .ToHashSet();

        foreach (var (property, value) in partialEntity.Properties)
        {
            property.SetValue(entity, value);

            if (references.Contains(property.Name))
            {
                DbContext.Entry(entity).Reference(property.Name).IsModified = true;
            }
            else
            {
                DbContext.Entry(entity).Property(property.Name).IsModified = true;
            }
        }
        
        return ValueTask.CompletedTask;
    }

    public virtual ValueTask UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Update(entity);
        
        return ValueTask.CompletedTask;
    }

    public virtual async ValueTask RemoveAsync(Guid id, bool softRemove = true, CancellationToken cancellationToken = default)
    {
        var dbSet = DbContext.Set<TEntity>();

        if (softRemove)
        {
            var partialEntity = new PartialEntity<TEntity>();
            partialEntity.AddValue(entity => entity.IsActive, false);
            
            await PartiallyUpdateAsync(id, partialEntity, cancellationToken);
        }
        else
        {
            TEntity entity = new() { Id = id };
            dbSet.Attach(entity);
            dbSet.Remove(entity);
        }
    }

    public virtual async Task<TEntity> FirstAsync(
        Expression<Func<TEntity, bool>>? expression = null, 
        Expression<Func<TEntity, object?>>? include = null,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await FirstOrDefaultAsync(expression, include, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.Create<TEntity>();
        }

        return entity;
    }

    public virtual async Task<TEntity> FirstAsync(
        Guid id,
        Expression<Func<TEntity, object?>>? include = null,
        CancellationToken cancellationToken = default
        )
    {
        return await FirstAsync(entity => entity.Id == id, include, cancellationToken);
    }

    public virtual async Task<TModel> FirstAsync<TModel>(
        Expression<Func<TEntity, bool>>? expression = null, 
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await FirstOrDefaultAsync<TModel>(expression, includes, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.Create<TEntity>();
        }

        return entity;
    }

    public virtual async Task<TModel> FirstAsync<TModel>(
        Guid id, 
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        CancellationToken cancellationToken = default
        )
    {
        return await FirstAsync<TModel>(entity => entity.Id == id, includes, cancellationToken);
    }

    public virtual async Task<TModel?> FirstOrDefaultAsync<TModel>(
        Expression<Func<TEntity, bool>>? expression = null, 
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        CancellationToken cancellationToken = default
        )
    {
        var entities = DbContext.Set<TEntity>().AsNoTracking();
            
        if (includes is not null)
        {
            entities = includes.Aggregate(entities, (queryable, include) => queryable.Include(include));
        }

        return await entities.Where(expression ?? (_ => true))
            .ProjectTo<TModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TModel?> FirstOrDefaultAsync<TModel>(
        Guid id, 
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        CancellationToken cancellationToken = default
        )
    {
        return await FirstOrDefaultAsync<TModel?>(entity => entity.Id == id, includes, cancellationToken);
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? expression = null, 
        Expression<Func<TEntity, object?>>? include = null,
        CancellationToken cancellationToken = default
        )
    {
        var entities = DbContext.Set<TEntity>().AsNoTracking();

        if (include is not null)
        {
            entities = entities.Include(include);
        }

        return await entities.FirstOrDefaultAsync(expression ?? (_ => true), cancellationToken);
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Guid id, Expression<Func<TEntity, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(entity => entity.Id == id, include, cancellationToken);
    }

    public virtual async Task<List<TModel>> WhereAsync<TModel>(
        ICollection<Expression<Func<TEntity, bool>>>? expressions = null,
        CancellationToken cancellationToken = default
        )
    {
        var entities = DbContext.Set<TEntity>().AsNoTracking();

        if (expressions is not null)
        {
            entities = expressions.Aggregate(entities, (queryable, expression) => queryable.Where(expression));
        }

        return await entities
            .ProjectTo<TModel>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<TEntity>> WhereAsync(
        ICollection<Expression<Func<TEntity, bool>>>? expressions = null, 
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        CancellationToken cancellationToken = default
        )
    {
        var entities = DbContext.Set<TEntity>().AsNoTracking();

        if (expressions is not null)
        {
            entities = expressions.Aggregate(entities, (queryable, expression) => queryable.Where(expression));
        }
        
        if (includes is not null)
        {
            entities = includes.Aggregate(entities, (queryable, include) => queryable.Include(include));
        }

        return await entities.ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<IPaginatedList<TModel>> PaginateAsync<TModel>(
        uint page, 
        uint pageSize,
        uint offset = 0,
        ICollection<Expression<Func<TEntity, object?>>>? includes = null,
        ICollection<Expression<Func<TEntity, bool>>>? expressions = null,
        CancellationToken cancellationToken = default
        )
    {
        var entities = DbContext.Set<TEntity>().AsNoTracking();

        if (expressions is not null)
        {
            entities = expressions.Aggregate(entities, (queryable, expression) => queryable.Where(expression));
        }
        
        if (includes is not null)
        {
            entities = includes.Aggregate(entities, (queryable, include) => queryable.Include(include));
        }

        return await entities
            .ProjectTo<TModel>(Mapper.ConfigurationProvider)
            .PaginateByPageSizeAsync(page, pageSize, offset, cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await AnyAsync(entity => entity.Id == id, cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>().AnyAsync(expression ?? (_ => true), cancellationToken: cancellationToken);
    }

    public virtual async Task<int> CountAsync(ICollection<Expression<Func<TEntity, bool>>>? expressions = null, CancellationToken cancellationToken = default)
    {
        var entities = DbContext.Set<TEntity>().AsNoTracking();

        if (expressions is not null)
        {
            entities = expressions.Aggregate(entities, (queryable, expression) => queryable.Where(expression));
        }
        
        return await entities.CountAsync(cancellationToken: cancellationToken);
    }
}