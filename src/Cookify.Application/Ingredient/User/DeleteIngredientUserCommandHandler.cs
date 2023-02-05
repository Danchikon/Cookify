using Cookify.Application.Common.Cqrs;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.Ingredient;
using Cookify.Domain.IngredientUser;
using MediatR;

namespace Cookify.Application.Ingredient.User;

public record DeleteIngredientUserCommandHandler : ICommandHandler<DeleteIngredientUserCommand>
{
    private readonly IIngredientsRepository _ingredientsRepository;
    private readonly IIngredientUsersRepository _ingredientUsersRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteIngredientUserCommandHandler(
        IIngredientsRepository ingredientsRepository,
        IIngredientUsersRepository ingredientUsersRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork
    )
    {
        _ingredientsRepository = ingredientsRepository;
        _ingredientUsersRepository = ingredientUsersRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(DeleteIngredientUserCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var ingredientUser = await _ingredientUsersRepository.FirstAsync(userId, command.IngredientId);

        await _ingredientUsersRepository.RemoveAsync(ingredientUser);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}