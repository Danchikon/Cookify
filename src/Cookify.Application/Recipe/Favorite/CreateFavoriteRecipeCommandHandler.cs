using Cookify.Application.Common.Cqrs;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Services;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.Favorite;
using Cookify.Domain.Recipe;
using MediatR;

namespace Cookify.Application.Recipe.Favorite;

public record CreateFavoriteRecipeCommandHandler : ICommandHandler<CreateFavoriteRecipeCommand>
{
    private readonly IRecipesRepository _recipesRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFavoriteRecipeCommandHandler(
        IRecipesRepository recipesRepository, 
        ICurrentUserService currentUserService,
        IFavoritesRepository favoritesRepository,
        IUnitOfWork unitOfWork
        )
    {
        _recipesRepository = recipesRepository;
        _currentUserService = currentUserService;
        _favoritesRepository = favoritesRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(CreateFavoriteRecipeCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        if (!await _recipesRepository.AnyAsync(command.RecipeId))
        {
            throw NotFoundException.Create<RecipeEntity>(command.RecipeId);
        }

       await _favoritesRepository.AddAsync(new FavoriteEntity(command.RecipeId, userId));
       await _unitOfWork.SaveChangesAsync();
       
       return Unit.Value;
    }
}