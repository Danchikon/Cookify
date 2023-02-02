using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos;
using Cookify.Application.Models;
using Cookify.Application.Services;

namespace Cookify.Application.User.Avatar;

public class GetUserAvatarLinkQueryHandler : IQueryHandler<GetUserAvatarLinkQuery, string>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;

    public GetUserAvatarLinkQueryHandler(IFileStorageService fileStorageService, ICurrentUserService currentUserService)
    {
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
    }
    
    public Task<string> Handle(GetUserAvatarLinkQuery query, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        var fileName = $"users/{userId}/avatar";

        var avatarLink = _fileStorageService.GetFileLink(fileName);
        
        return Task.FromResult(avatarLink);
    }
}