using Cookify.Application.Common.Pagination;
using Cookify.Domain.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Infrastructure.Common.Extensions;

public static class PaginationExtensions
{
    public static async Task<IPaginatedList<TEntity>> PaginateByPageSizeAsync<TEntity>(
        this IQueryable<TEntity> source,
        uint currentPage, 
        uint pageSize,
        uint offset = 0
    )
    {
        var totalCount = await source.CountAsync();
        var items = await source
            .Skip((int)(pageSize * (currentPage - 1) + offset))
            .Take((int)pageSize)
            .ToListAsync();

        return new PaginatedList<TEntity>(items, (uint)totalCount, currentPage, offset);
    }
    
    public static IPaginatedList<TEntity> PaginateByPageSize<TEntity>(
        this ICollection<TEntity> source,
        uint currentPage, 
        uint pageSize,
        uint offset = 0
    )
    {
        var totalCount = source.Count;
        var items = source
            .Skip((int) (pageSize * (currentPage - 1) + offset))
            .Take((int) pageSize)
            .ToList();

        return new PaginatedList<TEntity>(items, (uint)totalCount, currentPage, offset);
    }
}