using Cookify.Application.Common.Cqrs;
using Cookify.Application.Expressions;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Like;
using MediatR;

namespace Cookify.Application.Recipe.Like;

public record DeleteLikeRecipeCommandHandler : ICommandHandler<DeleteLikeRecipeCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILikesRepository _likesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLikeRecipeCommandHandler(
        ICurrentUserService currentUserService,
        ILikesRepository likesRepository,
        IUnitOfWork unitOfWork
        )
    {
        _currentUserService = currentUserService;
        _likesRepository = likesRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeleteLikeRecipeCommand command, CancellationToken cancellationToken)
    { 
        var userId = _currentUserService.GetUserId();
        var favorite = await _likesRepository.FirstAsync(LikeExpressions.RecipeIdAndCreatedByEquals(command.RecipeId, userId));

        await _likesRepository.RemoveAsync(favorite.Id, false); 
        await _unitOfWork.SaveChangesAsync();
        
        return Unit.Value;
    }
}