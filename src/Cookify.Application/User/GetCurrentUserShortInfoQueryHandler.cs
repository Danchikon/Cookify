using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Dtos.User;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.User;

namespace Cookify.Application.User;

public class GetCurrentUserQueryShortInfoHandler : IQueryHandler<GetCurrentUserShortInfoQuery, UserShortInfoDto>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCurrentUserQueryShortInfoHandler(IUsersRepository usersRepository, ICurrentUserService currentUserService)
    {
        _usersRepository = usersRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<UserShortInfoDto> Handle(GetCurrentUserShortInfoQuery query, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var user = await _usersRepository.FirstAsync<UserShortInfoDto>(userId, new []
        {
            UserExpressions.Likes(),
            UserExpressions.Favorites(),
            UserExpressions.IngredientUsers()
        });
        
        return user;
    }
}