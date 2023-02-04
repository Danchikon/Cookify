using Cookify.Application.Services;
using Cookify.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Cookify.Infrastructure.Services;

public class HttpCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthenticationService _authenticationService;
    
    public HttpCurrentUserService(IHttpContextAccessor httpContextAccessor, IAuthenticationService authenticationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authenticationService = authenticationService;
    }
    
    public Guid GetUserId()
    {
        var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.Single();

        if (string.IsNullOrWhiteSpace(authorizationHeader))
        {
            throw UnauthorizedException.Create();
        } 
        
        var jsonWebToken = authorizationHeader.Split(' ').Last();
        var claims = _authenticationService.GetClaimsPrincipal(jsonWebToken)?.Claims;
        var userIdString = claims?.Single(claim => claim.Type == "userId").Value;

        if (string.IsNullOrWhiteSpace(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw UnauthorizedException.Create();
        }

        return userId;
    }
}