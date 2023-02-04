using Cookify.Domain.Common.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Infrastructure.UnitOfWork;

public class EfUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;
    
    public EfUnitOfWork(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IAsyncDisposable> StartTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _dbContext.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}