namespace Cookify.Domain.Common.UnitOfWork;

public interface IUnitOfWork
{
    public Task<IAsyncDisposable> StartTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
    public Task SaveChangesAsync();
}