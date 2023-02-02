using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cookify.Domain.Common.Entities;
using Cookify.Domain.Common.Exceptions;
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
    private readonly TContext _dbContext;
    private readonly IMapper _mapper;

    public EfRepository(TContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public virtual async Task<Guid> AddAsync(TEntity entity)
    {
        var result = await _dbContext.Set<TEntity>().AddAsync(entity);
        return result.Entity.Id;
    }

    public virtual ValueTask PartiallyUpdateAsync(Guid id, PartialEntity<TEntity> partialEntity)
    {
        TEntity entity = new() { Id = id };
        
        _dbContext.Set<TEntity>().Attach(entity);
        
        var references = _dbContext.Entry(entity)
                .References
                .Select(x => x.Metadata.Name)
                .ToHashSet();

        foreach (var (property, value) in partialEntity.Properties)
        {
            property.SetValue(entity, value);

            if (references.Contains(property.Name))
            {
                _dbContext.Entry(entity).Reference(property.Name).IsModified = true;
            }
            else
            {
                _dbContext.Entry(entity).Property(property.Name).IsModified = true;
            }
        }
        
        return ValueTask.CompletedTask;
    }

    public virtual ValueTask UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        
        return ValueTask.CompletedTask;
    }

    public virtual ValueTask RemoveAsync(Guid id)
    {
        TEntity entity = new() { Id = id };
        _dbContext.Set<TEntity>().Attach(entity);
        _dbContext.Set<TEntity>().Remove(entity);
        
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

    public async Task<TEntity> FirstAsync(Guid id)
    {
        return await FirstAsync(entity => entity.Id == id);
    }

    public async Task<TModel> FirstAsync<TModel>(Expression<Func<TEntity, bool>>? expression = null)
    {
        var entity = await FirstOrDefaultAsync<TModel>(expression);

        if (entity is null)
        {
            throw NotFoundException.Create<TEntity>();
        }

        return entity;
    }

    public async Task<TModel> FirstAsync<TModel>(Guid id)
    {
        return await FirstAsync<TModel>(entity => entity.Id == id);
    }

    public virtual async Task<TModel?> FirstOrDefaultAsync<TModel>(Expression<Func<TEntity, bool>>? expression = null)
    {
        var entity = await _dbContext.Set<TEntity>()
            .AsNoTracking()
            .Where(expression ?? (_ => true))
            .ProjectTo<TModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return entity;
    }

    public virtual async Task<TModel?> FirstOrDefaultAsync<TModel>(Guid id)
    {
        return await FirstOrDefaultAsync<TModel?>(entity => entity.Id == id);
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        var entity = await _dbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(expression ?? (_ => true));

        return entity;
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Guid id)
    {
        return await FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public virtual async Task<List<TModel>> WhereAsync<TModel>(Expression<Func<TEntity, bool>>? expression = null)
    {
        var entities = await _dbContext.Set<TEntity>()
            .AsNoTracking()
            .Where(expression ?? (_ => true))
            .ProjectTo<TModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return entities;

    }

    public virtual async Task<IPaginatedList<TModel>> PaginateAsync<TModel>(
        uint currentPage, 
        uint pageSize,
        uint offset = 0,
        Expression<Func<TEntity, bool>>[]? expressions = null
        )
    {
        var entities = _dbContext.Set<TEntity>().AsNoTracking();

        if (expressions is not null)
        {
            entities = expressions.Aggregate(entities, (queryable, expression) => queryable.Where(expression));
        }

        return await entities
            .ProjectTo<TModel>(_mapper.ConfigurationProvider)
            .PaginateByPageSizeAsync(currentPage, pageSize, offset);
    }

    public virtual async Task<bool> AnyAsync(Guid id)
    {
        return await AnyAsync(entity => entity.Id == id);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        return await _dbContext.Set<TEntity>().AnyAsync(expression ?? (_ => true));
    }
}