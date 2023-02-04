using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;
using Cookify.Application.Dtos.Authentication;

namespace Cookify.Application.User.Authentication;

public record AuthenticateUserCommand(string Username, string Password) : CommandBase<JsonWebTokenDto>;