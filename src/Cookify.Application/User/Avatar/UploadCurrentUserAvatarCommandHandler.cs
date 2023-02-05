using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Dtos;
using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Domain.Common.Entities;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.User;

namespace Cookify.Application.User.Avatar;

public class UploadCurrentUserAvatarCommandHandler : ICommandHandler<UploadCurrentUserAvatarCommand, string>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadCurrentUserAvatarCommandHandler(
        IFileStorageService fileStorageService, 
        ICurrentUserService currentUserService,
        IUsersRepository usersRepository,
        IUnitOfWork unitOfWork
    )
    {
        _usersRepository = usersRepository;
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<string> Handle(UploadCurrentUserAvatarCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _currentUserService.GetUserId();
            var fileName = FileNameFormatter.FormatForUserAvatar(userId);
        
            var avatarLink = await _fileStorageService.PutFileAsync(new FileModel(
                command.FileStream, 
                command.ContentType, 
                fileName
            ));

            var partialUser = new PartialEntity<UserEntity>();
            partialUser.AddValue(user => user.AvatarImageLink, avatarLink);
            await _usersRepository.PartiallyUpdateAsync(userId, partialUser);
            await _unitOfWork.SaveChangesAsync();

            return avatarLink;
        }
        finally
        {
            await command.DisposeAsync();
        }
    }
}