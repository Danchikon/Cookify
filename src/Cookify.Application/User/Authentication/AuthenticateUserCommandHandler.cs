using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;
using Cookify.Application.Services;
using Cookify.Domain.Exceptions;
using Cookify.Domain.User;

namespace Cookify.Application.User.Authentication;

public class AuthenticateUserCommandHandler : ICommandHandler<AuthenticateUserCommand, JsonWebTokenDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    
    public AuthenticateUserCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        // _userRepository = userRepository;
    }
    
    public async Task<JsonWebTokenDto> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        // var user = await _userRepository.FirstOrDefaultAsync(user => user.Username == command.Username);
        //
        // if (user is null)
        // {
        //     throw NotAuthenticatedException.Create(command.Username);
        // }
        //
        // if (!_authenticationService.ValidatePassword(command.Password, user.PasswordHash))
        // {
        //     throw NotAuthenticatedException.Create(command.Username);
        // }

        return _authenticationService.AuthenticateUser(new UserEntity
        {
            Id = Guid.NewGuid(),
            Username = "Daniel",
            Email = "patikotmot@gmail.com"
        });
    }
}