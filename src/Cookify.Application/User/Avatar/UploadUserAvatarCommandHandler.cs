using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;
using Cookify.Application.Models;
using Cookify.Application.Services;

namespace Cookify.Application.User.Avatar;

public class UploadUserAvatarCommandHandler : ICommandHandler<UploadUserAvatarCommand, string>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;

    public UploadUserAvatarCommandHandler(IFileStorageService fileStorageService, ICurrentUserService currentUserService)
    {
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
    }
    
    public async Task<string> Handle(UploadUserAvatarCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var fileName = $"users/{userId}/avatar"; 
        
        var avatarLink = await _fileStorageService.PutFileAsync(new FileModel(
            command.FileStream, 
            command.ContentType, 
            fileName
            ));
        
        await command.DisposeAsync();

        return avatarLink;
    }
}