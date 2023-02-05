using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Authentication;

namespace Cookify.Application.User.Registration;

public record RegisterUserCommand(
    string Username, 
    string Email,
    string Password,
    string PasswordConfirmation
    ) : CommandBase<JsonWebTokenDto>;