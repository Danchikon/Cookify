using MediatR;

namespace Tipsy.Skeleton.Application.CQRS;

public interface IQuery<out TResult> : IRequest<TResult>
{
    
}