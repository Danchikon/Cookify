using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;
using Cookify.Application.Dtos.Authentication;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.Session;
using Cookify.Domain.User;
using MediatR;

namespace Cookify.Application.User.Authentication;

public class DeleteCurrentUserSessionCommandHandler : ICommandHandler<DeleteCurrentUserSessionCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUsersRepository _userRepository;
    private readonly ISessionsRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteCurrentUserSessionCommandHandler(
        ICurrentUserService currentUserService,
        ISessionsRepository sessionRepository,
        IUsersRepository userRepository,
        IUnitOfWork unitOfWork
        )
    {
        _currentUserService = currentUserService;
        _sessionRepository = sessionRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeleteCurrentUserSessionCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        
        var user = await _userRepository.FirstAsync(userId, UserExpressions.Session());

        if (user.Session is null)
        {
            return Unit.Value;
        }
        
        await _sessionRepository.RemoveAsync(user.Session.Id, false);
        user.SessionId = null;
        user.Session = null;
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}