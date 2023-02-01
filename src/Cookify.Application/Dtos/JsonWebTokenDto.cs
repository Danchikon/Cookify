namespace Cookify.Application.Dtos;

public record JsonWebTokenDto
{
    public string JsonWebToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}