using MediatR;

namespace Cookify.Application.Common.Cqrs;

public abstract record QueryBase<TResult> : IRequest<TResult>
{
    
}