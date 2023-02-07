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
        
        var like = await _likesRepository.FirstAsync(
            LikeExpressions.RecipeIdAndCreatedByEquals(command.RecipeId, userId), 
            cancellationToken: cancellationToken
            );

        await _likesRepository.RemoveAsync(like.Id, false, cancellationToken); 
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}