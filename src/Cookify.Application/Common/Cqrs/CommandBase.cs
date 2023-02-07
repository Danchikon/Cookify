using MediatR;

namespace Cookify.Application.Common.Cqrs;

public abstract record CommandBase : CommandBase<Unit>, IRequest
{
    
}

public abstract record CommandBase<TResult> : IRequest<TResult>
{
    
}