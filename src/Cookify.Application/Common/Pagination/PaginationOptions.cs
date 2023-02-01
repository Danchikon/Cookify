namespace Cookify.Application.Common.Pagination;

public class PaginationOptions 
{
    public const uint DefaultCurrentPage = 1;
    public const uint DefaultPageSize = 5;
    public uint CurrentPage { get; set; } = DefaultCurrentPage;
    public uint PageSize { get; set; } = DefaultPageSize;
}