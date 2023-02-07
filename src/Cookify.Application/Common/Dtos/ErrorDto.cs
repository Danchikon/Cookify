using Cookify.Domain.Common.Enums;

namespace Cookify.Application.Common.Dtos;

public record ErrorDto
{
    public string Title { get; init;  } = string.Empty;
    public int Code { get; init; }
    public ICollection<string> Messages { get; init; } = Array.Empty<string>();
}