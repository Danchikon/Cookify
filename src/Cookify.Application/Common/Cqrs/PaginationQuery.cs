using Cookify.Application.Common.Pagination;

namespace Cookify.Application.Common.Cqrs;

public record PaginationQuery<TResult>(PaginationOptions Pagination) : QueryBase<PaginatedList<TResult>> 
{
    public uint CurrentPage => Pagination.CurrentPage;
    public uint PageSize => Pagination.PageSize;
}