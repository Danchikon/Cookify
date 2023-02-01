using Cookify.Domain.Common.Pagination;

namespace Cookify.Application.Common.Pagination;

public class PaginatedList<TEntity> : IPaginatedList<TEntity>
{
    public uint TotalCount { get; private set; }
    public uint Count { get; private set; }
    public uint Page { get; private set; }
    public uint Offset { get; private set;  }
    public ICollection<TEntity> Items { get; private set; }

    public PaginatedList(ICollection<TEntity> items, uint totalCount, uint page, uint offset)
    {
        TotalCount = totalCount;
        Items = items;
        Count = (uint) items.Count;
        Page = page;
        Offset = offset;
    }
}