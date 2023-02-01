using MediatR;
using Tipsy.Skeleton.Application.CQRS;

namespace Cookify.Application.Common.Cqrs;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    
}