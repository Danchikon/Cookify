namespace Cookify.Application.Dtos.User;

public record UserShortInfoDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
    public string Username { get; init; } = null!;
}