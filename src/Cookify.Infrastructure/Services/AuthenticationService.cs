using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Cookify.Application.Common.Constants;
using Cookify.Application.Dtos;
using Cookify.Application.Dtos.Authentication;
using Cookify.Application.Services;
using Cookify.Domain.User;
using Cookify.Infrastructure.Options;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cookify.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationOptions _options;
    
    public AuthenticationService(IOptions<AuthenticationOptions> options)
    {
        _options = options.Value;
    }
    
    public bool ValidatePassword(string password, string passwordHash)
    {
        var hashed = GetPasswordHash(password);

        return hashed == passwordHash;
    }
    
    public bool ValidateRefreshToken(string refreshToken, string refreshTokenHash)
    {
        var hashed = GetRefreshTokenHash(refreshToken);

        return hashed == refreshTokenHash;
    }

    public string GetPasswordHash(string password)
    {
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.UTF8.GetBytes("passwordSalt"),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));

        return hashed;
    }

    public string GetRefreshTokenHash(string refreshToken)
    {
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: refreshToken,
            salt: Encoding.UTF8.GetBytes("refreshTokenSalt"),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));

        return hashed;
    }

    public ClaimsPrincipal? GetClaimsPrincipal(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (
            securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256)
        )
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;

    }

    public JsonWebTokenDto AuthenticateUser(UserEntity user, string? refreshToken = null)
    {
        var now = DateTime.UtcNow;
        var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
        
        var jwt = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            notBefore: now,
            claims: new []
            {
                new Claim(UserClaimsConstants.UserId, user.Id.ToString()),
                new Claim(UserClaimsConstants.Username, user.Username),
                new Claim(UserClaimsConstants.Email, user.Email)
            },
            expires: now.Add(TimeSpan.FromMinutes(_options.Lifetime)),
            signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
            );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return new JsonWebTokenDto
            {
                JsonWebToken = encodedJwt,
                RefreshToken = GenerateRefreshToken()
            };
        }

        return new JsonWebTokenDto
        {
            JsonWebToken = encodedJwt,
            RefreshToken = refreshToken
        };
    }
    
    public string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}