using Cookify.Application.Common.Cqrs;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.Like;
using Cookify.Domain.Recipe;
using MediatR;

namespace Cookify.Application.Recipe.Like;

public record CreateLikeRecipeCommandHandler : ICommandHandler<CreateLikeRecipeCommand>
{
    private readonly IRecipesRepository _recipesRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILikesRepository _likesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLikeRecipeCommandHandler(
        IRecipesRepository recipesRepository, 
        ICurrentUserService currentUserService,
        ILikesRepository likesRepository,
        IUnitOfWork unitOfWork
        )
    {
        _recipesRepository = recipesRepository;
        _currentUserService = currentUserService;
        _likesRepository = likesRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(CreateLikeRecipeCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        if (!await _recipesRepository.AnyAsync(command.RecipeId))
        {
            throw NotFoundException.Create<RecipeEntity>(command.RecipeId);
        }

       await _likesRepository.AddAsync(new LikeEntity(command.RecipeId, userId));
       await _unitOfWork.SaveChangesAsync();
       
       return Unit.Value;
    }
}