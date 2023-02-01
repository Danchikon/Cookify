namespace Cookify.Application.Dtos;

public record UserShortInfoDto
{
    public Guid Id { get; init; }
    public string Username { get; init; } = null!;
}