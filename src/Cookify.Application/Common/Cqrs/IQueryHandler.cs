using MediatR;

namespace Cookify.Application.Common.Cqrs;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : QueryBase<TResult>
{
    
}