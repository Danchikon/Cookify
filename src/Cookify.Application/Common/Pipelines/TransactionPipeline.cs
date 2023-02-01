using Cookify.Application.Common.Cqrs;
using Cookify.Domain.Common.UnitOfWork;
using MediatR;

namespace Cookify.Application.Common.Pipelines;

public class TransactionPipeline<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> 
    where TCommand : ICommand<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionPipeline(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.StartTransactionAsync();
            var result = await next();
            await _unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch (Exception) 
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}