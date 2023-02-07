using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Services;
using Cookify.Domain.Common.Entities;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.User;
using MediatR;

namespace Cookify.Application.User.Avatar;

public class DeleteCurrentUserAvatarCommandHandler : ICommandHandler<DeleteCurrentUserAvatarCommand>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCurrentUserAvatarCommandHandler(
        IFileStorageService fileStorageService, 
        ICurrentUserService currentUserService,
        IUsersRepository usersRepository,
        IUnitOfWork unitOfWork
        )
    {
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeleteCurrentUserAvatarCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var fileName = FileNameFormatter.FormatForUserAvatar(userId); 
        
        await _fileStorageService.RemoveFileAsync(fileName, cancellationToken);
        
        var partialUser = new PartialEntity<UserEntity>()
            .AddValue(user => user.AvatarImageLink, null);
        
        await _usersRepository.PartiallyUpdateAsync(userId, partialUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}