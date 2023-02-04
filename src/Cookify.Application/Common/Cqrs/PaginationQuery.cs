using Cookify.Application.Common.Pagination;
using Cookify.Domain.Common.Pagination;

namespace Cookify.Application.Common.Cqrs;

public record PaginationQuery<TResult>(PaginationOptions Pagination) : QueryBase<IPaginatedList<TResult>>;