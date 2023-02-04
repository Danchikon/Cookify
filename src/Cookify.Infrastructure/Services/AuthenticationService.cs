using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.UTF8.GetBytes("salt"),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
            ));

        return hashed == passwordHash;
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

    public JsonWebTokenDto AuthenticateUser(UserEntity user)
    {
        var now = DateTime.UtcNow;
        var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
        
        var jwt = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            notBefore: now,
            claims: new []
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("email", user.Email)
            },
            expires: now.Add(TimeSpan.FromMinutes(_options.Lifetime)),
            signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
            );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        var refreshToken = GenerateRefreshToken();

        return new JsonWebTokenDto
        {
            JsonWebToken = encodedJwt,
            RefreshToken = refreshToken
        };
    }
    
    private static string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}