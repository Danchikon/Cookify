using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Authentication;

namespace Cookify.Application.User.Authentication;

public record RefreshJsonWebTokenCommand(string JsonWebToken, string RefreshToken) : CommandBase<JsonWebTokenDto>;