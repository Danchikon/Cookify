namespace Cookify.Application.Dtos.Authentication;

public record JsonWebTokenDto
{
    public string JsonWebToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}