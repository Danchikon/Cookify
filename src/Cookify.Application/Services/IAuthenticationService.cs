using Cookify.Application.Dtos;
using Cookify.Domain.User;

namespace Cookify.Application.Services;

public interface IAuthenticationService
{
    JsonWebTokenDto AuthenticateUser(UserEntity user);
}