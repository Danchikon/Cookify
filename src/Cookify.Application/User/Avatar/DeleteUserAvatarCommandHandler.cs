using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;
using Cookify.Application.Services;
using MediatR;

namespace Cookify.Application.User.Avatar;

public class DeleteUserAvatarCommandHandler : ICommandHandler<DeleteUserAvatarCommand>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUserAvatarCommandHandler(IFileStorageService fileStorageService, ICurrentUserService currentUserService)
    {
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
    }
    
    public async Task<Unit> Handle(DeleteUserAvatarCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var fileName = $"users/{userId}/avatar"; 
        
        await _fileStorageService.RemoveFileAsync(fileName);

        return Unit.Value;
    }
}