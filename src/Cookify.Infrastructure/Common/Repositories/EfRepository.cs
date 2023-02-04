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
    
    public virtual async Task<Guid> AddAsync(TEntity entity)
    {
        var result = await DbContext.Set<TEntity>().AddAsync(entity);
        return result.Entity.Id;
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(entities);
    }

    public virtual ValueTask PartiallyUpdateAsync(Guid id, PartialEntity<TEntity> partialEntity)
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

    public virtual ValueTask UpdateAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
        
        return ValueTask.CompletedTask;
    }

    public virtual ValueTask RemoveAsync(Guid id)
    {
        TEntity entity = new() { Id = id };
        DbContext.Set<TEntity>().Attach(entity);
        DbContext.Set<TEntity>().Remove(entity);
        
        return ValueTask.CompletedTask;
    }

    public virtual async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        var entity = await FirstOrDefaultAsync(expression);

        if (entity is null)
        {
            throw NotFoundException.Create<TEntity>();
        }

        return entity;
    }

    public virtual async Task<TEntity> FirstAsync(Guid id)
    {
        return await FirstAsync(entity => entity.Id == id);
    }

    public virtual async Task<TModel> FirstAsync<TModel>(Expression<Func<TEntity, bool>>? expression = null)
    {
        var entity = await FirstOrDefaultAsync<TModel>(expression);

        if (entity is null)
        {
            throw NotFoundException.Create<TEntity>();
        }

        return entity;
    }

    public virtual async Task<TModel> FirstAsync<TModel>(Guid id)
    {
        return await FirstAsync<TModel>(entity => entity.Id == id);
    }

    public virtual async Task<TModel?> FirstOrDefaultAsync<TModel>(Expression<Func<TEntity, bool>>? expression = null)
    {
        var entity = await DbContext.Set<TEntity>()
            .AsNoTracking()
            .Where(expression ?? (_ => true))
            .ProjectTo<TModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return entity;
    }

    public virtual async Task<TModel?> FirstOrDefaultAsync<TModel>(Guid id)
    {
        return await FirstOrDefaultAsync<TModel?>(entity => entity.Id == id);
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        var entity = await DbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(expression ?? (_ => true));

        return entity;
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Guid id)
    {
        return await FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public virtual async Task<List<TModel>> WhereAsync<TModel>(params Expression<Func<TEntity, bool>>[]? expressions)
    {
        var entities = DbContext.Set<TEntity>().AsNoTracking();

        if (expressions is not null)
        {
            entities = expressions.Aggregate(entities, (queryable, expression) => queryable.Where(expression));
        }

        return await entities
            .ProjectTo<TModel>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public virtual async Task<List<TEntity>> WhereAsync(params Expression<Func<TEntity, bool>>[]? expressions)
    {
        var entities = DbContext.Set<TEntity>().AsNoTracking();

        if (expressions is not null)
        {
            entities = expressions.Aggregate(entities, (queryable, expression) => queryable.Where(expression));
        }

        return await entities.ToListAsync();
    }

    public virtual async Task<IPaginatedList<TModel>> PaginateAsync<TModel>(
        uint page, 
        uint pageSize,
        uint offset = 0,
        ICollection<Expression<Func<TEntity, object>>>? includes = null,
        ICollection<Expression<Func<TEntity, bool>>>? expressions = null
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
            .PaginateByPageSizeAsync(page, pageSize, offset);
    }

    public virtual async Task<bool> AnyAsync(Guid id)
    {
        return await AnyAsync(entity => entity.Id == id);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        return await DbContext.Set<TEntity>().AnyAsync(expression ?? (_ => true));
    }
}