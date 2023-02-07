using Cookify.Application.Common.Cqrs;
using Cookify.Domain.Common.UnitOfWork;
using MediatR;

namespace Cookify.Application.Common.Pipelines;

public class AsyncDisposingPipeline<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> 
    where TCommand : CommandBase, IRequest<TResponse>, IAsyncDisposable
{
    public async Task<TResponse> Handle(TCommand command, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        finally
        {
            await command.DisposeAsync();
        }
    }
}