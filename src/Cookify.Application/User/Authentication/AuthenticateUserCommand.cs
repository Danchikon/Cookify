using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;

namespace Cookify.Application.User.Authentication;

public record AuthenticateUserCommand : ICommand<JsonWebTokenDto>
{
    public string Username { get; init; }
    public string Password { get; init; }
}