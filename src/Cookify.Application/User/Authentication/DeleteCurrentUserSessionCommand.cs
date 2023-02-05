using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Authentication;

namespace Cookify.Application.User.Authentication;

public record DeleteCurrentUserSessionCommand : CommandBase;