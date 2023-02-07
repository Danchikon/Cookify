using MediatR;

namespace Cookify.Application.Common.Cqrs;

public abstract record CommandBase : IRequest
{
    
}

public abstract record CommandBase<TResult> : CommandBase, IRequest<TResult>
{
    
}