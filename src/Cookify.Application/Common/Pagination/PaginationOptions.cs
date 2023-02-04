namespace Cookify.Application.Common.Pagination;

public class PaginationOptions 
{
    public const uint DefaultPage = 1;
    public const uint DefaultPageSize = 5;
    public const uint DefaultOffset = 0;
    public uint Page { get; set; } = DefaultPage;
    public uint PageSize { get; set; } = DefaultPageSize;
    public uint Offset { get; set; } = DefaultOffset;
}