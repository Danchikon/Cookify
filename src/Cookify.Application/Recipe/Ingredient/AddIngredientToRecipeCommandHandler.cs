using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Domain.Common.Exceptions;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Ingredient;
using Cookify.Domain.IngredientRecipe;
using Cookify.Domain.Recipe;
using MediatR;

namespace Cookify.Application.Recipe.Ingredient;

public record AddIngredientToRecipeCommandHandler : ICommandHandler<AddIngredientToRecipeCommand>
{
    private readonly IRecipesRepository _recipesRepository;
    private readonly IIngredientsRepository _ingredientsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIngredientRecipesRepository _ingredientRecipesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddIngredientToRecipeCommandHandler(
        IRecipesRepository recipesRepository,
        IIngredientsRepository ingredientsRepository,
        ICurrentUserService currentUserService,
        IIngredientRecipesRepository ingredientRecipesRepository,
        IUnitOfWork unitOfWork
    )
    {
        _recipesRepository = recipesRepository;
        _ingredientsRepository = ingredientsRepository;
        _currentUserService = currentUserService;
        _ingredientRecipesRepository = ingredientRecipesRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(AddIngredientToRecipeCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var recipeExists = await _recipesRepository.AnyAsync(command.RecipeId, cancellationToken);
        
        if (!recipeExists)
        {
            throw NotFoundException.Create<RecipeEntity>(command.RecipeId);
        }
        
        var recipe = await _recipesRepository.FirstOrDefaultAsync(recipe => recipe.CreatedBy == userId && recipe.Id == command.RecipeId, cancellationToken: cancellationToken);
        
        if (recipe is null)
        {
            throw UnauthorizedException.Create();
        }
        
        var ingredient = await _ingredientsRepository.FirstOrDefaultAsync(ingredient => ingredient.Id == command.IngredientId, cancellationToken: cancellationToken);
        
        if (ingredient is null)
        {
            throw NotFoundException.Create<IngredientEntity>();
        }

        await _ingredientRecipesRepository.AddAsync(
            new IngredientRecipeEntity(
                ingredient.Id, 
                recipe.Id, 
                command.UkrainianMeasure, 
                command.UkrainianMeasure
                ), cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}