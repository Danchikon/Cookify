using System.Linq.Expressions;
using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Dtos.User;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.User;

namespace Cookify.Application.User;

public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCurrentUserQueryHandler(IUsersRepository usersRepository, ICurrentUserService currentUserService)
    {
        _usersRepository = usersRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<UserDto> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var user = await _usersRepository.FirstAsync<UserDto>(userId, new []
        {
            UserExpressions.Recipes(),
            UserExpressions.Likes(),
            UserExpressions.Favorites(),
            UserExpressions.IngredientUsers()
        }, cancellationToken);
        
        return user;
    }
}