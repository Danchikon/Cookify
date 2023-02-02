using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;

namespace Cookify.Application.User.Authentication;

public record AuthenticateUserCommand : CommandBase<JsonWebTokenDto>
{
    public string Username { get; init; }
    public string Password { get; init; }
}