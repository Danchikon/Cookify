namespace Cookify.Domain.Common.Pagination;

public interface IPaginatedList<TEntity>
{
    public uint TotalCount { get; }
    public uint Count { get; }
    public ICollection<TEntity> Items { get; }
}