namespace Cookify.Domain.Common.UnitOfWork;

public interface IUnitOfWork
{
    public Task<IAsyncDisposable> StartTransactionAsync(CancellationToken cancellationToken);
    public Task CommitTransactionAsync(CancellationToken cancellationToken);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}