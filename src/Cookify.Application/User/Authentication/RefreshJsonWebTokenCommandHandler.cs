using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;
using Cookify.Application.Dtos.Authentication;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.Common.Exceptions;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.Exceptions.Users;
using Cookify.Domain.Session;
using Cookify.Domain.User;

namespace Cookify.Application.User.Authentication;

public class RefreshJsonWebTokenCommandHandler : ICommandHandler<RefreshJsonWebTokenCommand, JsonWebTokenDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUsersRepository _userRepository;
    private readonly ISessionsRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public RefreshJsonWebTokenCommandHandler(
        IAuthenticationService authenticationService,
        ISessionsRepository sessionRepository,
        ICurrentUserService currentUserService,
        IUsersRepository userRepository,
        IUnitOfWork unitOfWork
        )
    {
        _authenticationService = authenticationService;
        _sessionRepository = sessionRepository;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<JsonWebTokenDto> Handle(RefreshJsonWebTokenCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        
        var user = await _userRepository.FirstAsync(userId, UserExpressions.Session(), cancellationToken);

        if (user.Session is null)
        {
            throw UnauthenticatedException.Create();
        }
        
        if (user.Session.SessionExpirationTime < DateTimeOffset.UtcNow)
        {
            throw UnauthenticatedException.Create();
        }

        if (!_authenticationService.ValidateRefreshToken(command.RefreshToken, user.Session.RefreshTokenHash))
        {
            throw UnauthenticatedException.Create();
        }

        var refreshToken = _authenticationService.GenerateRefreshToken();
        user.Session.RefreshTokenHash = _authenticationService.GetRefreshTokenHash(refreshToken);
        await _sessionRepository.UpdateAsync(user.Session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _authenticationService.AuthenticateUser(user, refreshToken);
    }
}