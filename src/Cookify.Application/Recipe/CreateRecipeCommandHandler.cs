using System.Linq.Expressions;
using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Expressions;
using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Recipe;

namespace Cookify.Application.Recipe;

public record CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, Guid>
{
    private readonly IRecipesRepository _recipesRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRecipeCommandHandler(
        IRecipesRepository recipesRepository,
        IFileStorageService fileStorageService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork
    )
    {
        _recipesRepository = recipesRepository;
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Guid> Handle(CreateRecipeCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var recipe = new RecipeEntity(
            command.UkrainianTitle,
            command.UkrainianTitle,
            command.UkrainianInstruction,
            command.UkrainianInstruction,
            command.IsPublic,
            command.CategoryId,
            userId
        );

        var fileName = FileNameFormatter.FormatForRecipeImage(recipe.Id);
        recipe.ImageLink = _fileStorageService.GetFileLink(fileName);

        await _recipesRepository.AddAsync(recipe, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _fileStorageService.PutFileAsync(
            new FileModel(
                command.ImageStream, 
                command.ImageContentType, 
                fileName
                ),
                cancellationToken
            );

        return recipe.Id;
    }
}