using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.User;

namespace Cookify.Application.User;

public record GetCurrentUserQuery : QueryBase<UserDto>;