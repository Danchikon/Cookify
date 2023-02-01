using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;
using Cookify.Application.Services;
using Cookify.Domain.User;

namespace Cookify.Application.User.Authentication;

public class AuthenticateUserCommandHandler : ICommandHandler<AuthenticateUserCommand, JsonWebTokenDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    
    public AuthenticateUserCommandHandler(IAuthenticationService authenticationService, IUserRepository userRepository)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
    }
    
    public async Task<JsonWebTokenDto> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(user => user.Username == command.Username);
        
        
        
        return _authenticationService.AuthenticateUser();
    }
}