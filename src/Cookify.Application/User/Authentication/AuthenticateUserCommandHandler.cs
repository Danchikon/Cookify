using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;
using Cookify.Application.Dtos.Authentication;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.Session;
using Cookify.Domain.User;

namespace Cookify.Application.User.Authentication;

public class AuthenticateUserCommandHandler : ICommandHandler<AuthenticateUserCommand, JsonWebTokenDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUsersRepository _userRepository;
    private readonly ISessionsRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public AuthenticateUserCommandHandler(
        IAuthenticationService authenticationService,
        ISessionsRepository sessionRepository,
        IUsersRepository userRepository,
        IUnitOfWork unitOfWork
        )
    {
        _authenticationService = authenticationService;
        _sessionRepository = sessionRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<JsonWebTokenDto> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(
            expression: UserExpressions.UsernameEquals(command.Username),
            include: UserExpressions.Session()
        );
        
        if (user is null)
        {
            throw UnauthenticatedException.Create();
        }
        
        if (!_authenticationService.ValidatePassword(command.Password, user.PasswordHash))
        {
            throw UnauthenticatedException.Create();
        }

        if (user.Session is not null)
        {
            await _sessionRepository.RemoveAsync(user.Session.Id, false);
            user.SessionId = null;
            user.Session = null;
            await _userRepository.UpdateAsync(user);
        }
        
        var refreshToken = _authenticationService.GenerateRefreshToken();
        var refreshTokenHash = _authenticationService.GetRefreshTokenHash(refreshToken);
        var session = new SessionEntity(refreshTokenHash, user.Id, DateTimeOffset.UtcNow.AddMonths(12));
        user.SessionId = session.Id;
        
        await _sessionRepository.AddAsync(session);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        return _authenticationService.AuthenticateUser(user, refreshToken);
    }
}