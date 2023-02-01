using Cookify.Application.Common.Pagination;
using Tipsy.Skeleton.Application.CQRS;

namespace Cookify.Application.Common.Cqrs;

public record PaginationQuery<TResult>(PaginationOptions Pagination) : IQuery<PaginatedList<TResult>> 
{
    public uint CurrentPage => Pagination.CurrentPage;
    public uint PageSize => Pagination.PageSize;
}