using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.Favorite;
using Cookify.Domain.Recipe;
using MediatR;

namespace Cookify.Application.Recipe.Favorite;

public record DeleteFavoriteRecipeCommandHandler : ICommandHandler<DeleteFavoriteRecipeCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFavoriteRecipeCommandHandler(
        ICurrentUserService currentUserService,
        IFavoritesRepository favoritesRepository,
        IUnitOfWork unitOfWork
        )
    {
        _currentUserService = currentUserService;
        _favoritesRepository = favoritesRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeleteFavoriteRecipeCommand command, CancellationToken cancellationToken)
    { 
        var userId = _currentUserService.GetUserId();
        var favorite = await _favoritesRepository.FirstAsync(
            FavoriteExpressions.RecipeIdAndCreatedByEquals(command.RecipeId, userId), 
            cancellationToken: cancellationToken
            );

        await _favoritesRepository.RemoveAsync(favorite.Id, false, cancellationToken); 
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}