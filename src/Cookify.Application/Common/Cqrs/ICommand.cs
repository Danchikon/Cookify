using MediatR;

namespace Cookify.Application.Common.Cqrs;

public interface ICommand : IRequest
{
    
}

public interface ICommand<out TResult> : IRequest<TResult>
{
    
}