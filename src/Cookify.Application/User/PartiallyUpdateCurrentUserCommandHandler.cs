using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Dtos.User;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.Common.Entities;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.User;
using MediatR;

namespace Cookify.Application.User;

public class PartiallyUpdateCurrentUserCommandHandler : ICommandHandler<PartiallyUpdateCurrentUserCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public PartiallyUpdateCurrentUserCommandHandler(
        IUsersRepository usersRepository, 
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork
        
        )
    {
        _usersRepository = usersRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(PartiallyUpdateCurrentUserCommand query, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        if (!await  _usersRepository.AnyAsync(userId, cancellationToken))
        {
            throw NotFoundException.Create<UserEntity>(userId);
        }

        var partialEntity = new PartialEntity<UserEntity>()
            .AddValue(user => user.Email, query.Email, true)
            .AddValue(user => user.Username, query.Username, true);

        await _usersRepository.PartiallyUpdateAsync(userId, partialEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}