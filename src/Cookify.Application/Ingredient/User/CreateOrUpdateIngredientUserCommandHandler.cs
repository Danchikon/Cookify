using Cookify.Application.Common.Cqrs;
using Cookify.Application.Services;
using Cookify.Domain.Common.Exceptions;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Exceptions;
using Cookify.Domain.Ingredient;
using Cookify.Domain.IngredientUser;
using MediatR;

namespace Cookify.Application.Ingredient.User;

public record CreateOrUpdateIngredientUserCommandHandler : ICommandHandler<CreateOrUpdateIngredientUserCommand>
{
    private readonly IIngredientsRepository _ingredientsRepository;
    private readonly IIngredientUsersRepository _ingredientUsersRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrUpdateIngredientUserCommandHandler(
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
    
    public async Task<Unit> Handle(CreateOrUpdateIngredientUserCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();
        
        if (!await _ingredientsRepository.AnyAsync(command.IngredientId, cancellationToken))
        {
            throw NotFoundException.Create<IngredientEntity>(command.IngredientId);
        }

        var ingredientUser = await _ingredientUsersRepository.FirstOrDefaultAsync(userId, command.IngredientId, cancellationToken);
        
        if (ingredientUser is not null)
        {
            ingredientUser.UkrainianMeasure = command.UkrainianMeasure;
            await _ingredientUsersRepository.UpdateAsync(ingredientUser, cancellationToken);
        }
        else
        {
            await _ingredientUsersRepository.AddAsync(
                new IngredientUserEntity(
                    command.IngredientId, 
                    userId, 
                    command.UkrainianMeasure
                    ), 
                cancellationToken
                );
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}