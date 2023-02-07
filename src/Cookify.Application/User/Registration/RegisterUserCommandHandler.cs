using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Authentication;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.Session;
using Cookify.Domain.User;

namespace Cookify.Application.User.Registration;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, JsonWebTokenDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUsersRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public RegisterUserCommandHandler(
        IAuthenticationService authenticationService,
        IUsersRepository userRepository,
        IUnitOfWork unitOfWork
        )
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<JsonWebTokenDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var userAlreadyExists = await _userRepository.AnyAsync(UserExpressions.UsernameEquals(command.Username), cancellationToken);
        
        if (userAlreadyExists)
        {
            throw UserAlreadyExistsException.CreateForUsername(command.Username);
        }
        
        userAlreadyExists = await _userRepository.AnyAsync(UserExpressions.EmailEquals(command.Email), cancellationToken);
        
        if (userAlreadyExists)
        {
            throw UserAlreadyExistsException.CreateForEmail(command.Email);
        }

        if (command.Password != command.PasswordConfirmation)
        {
            throw PasswordsDoNotMatchException.Create();
        }
        
        var passwordHash = _authenticationService.GetPasswordHash(command.Password);
        var refreshToken = _authenticationService.GenerateRefreshToken();
        var refreshTokenHash = _authenticationService.GetRefreshTokenHash(refreshToken);
        
        var user = new UserEntity(command.Username, command.Email, passwordHash);
        var session = new SessionEntity(refreshTokenHash, user.Id, DateTimeOffset.UtcNow.AddMonths(12));
        user.Session = session;
        user.SessionId = session.Id;

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _authenticationService.AuthenticateUser(user, refreshToken);
    }
}