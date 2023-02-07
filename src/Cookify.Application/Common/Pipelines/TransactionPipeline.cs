using Cookify.Application.Common.Cqrs;
using Cookify.Domain.Common.UnitOfWork;
using MediatR;

namespace Cookify.Application.Common.Pipelines;

public class TransactionPipeline<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> 
    where TCommand : CommandBase, IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionPipeline(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TCommand command, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await using var _ = await _unitOfWork.StartTransactionAsync(cancellationToken);
            
            var result = await next();
            
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            
            return result;
        }
        catch (Exception) 
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}