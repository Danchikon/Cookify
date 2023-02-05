using System.Security.Claims;
using Cookify.Application.Dtos;
using Cookify.Application.Dtos.Authentication;
using Cookify.Domain.User;

namespace Cookify.Application.Services;

public interface IAuthenticationService
{
    bool ValidatePassword(string password, string passwordHash);
    bool ValidateRefreshToken(string refreshToken, string refreshTokenHash);
    string GetPasswordHash(string password);
    string GetRefreshTokenHash(string refreshToken);
    ClaimsPrincipal? GetClaimsPrincipal(string? token);
    JsonWebTokenDto AuthenticateUser(UserEntity user, string? refreshToken = null);
    public string GenerateRefreshToken();
}