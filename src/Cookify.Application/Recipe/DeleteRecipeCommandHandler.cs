using System.Linq.Expressions;
using Cookify.Application.Common.Cqrs;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Dtos.Recipe;
using Cookify.Application.Expressions;
using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Domain.Common.Exceptions;
using Cookify.Domain.Common.Pagination;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Recipe;
using MediatR;

namespace Cookify.Application.Recipe;

public record DeleteRecipeCommandHandler : ICommandHandler<DeleteRecipeCommand>
{
    private readonly IRecipesRepository _recipesRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRecipeCommandHandler(
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
    
    public async Task<Unit> Handle(DeleteRecipeCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserId();

        var recipeExists = await _recipesRepository.AnyAsync(recipe => recipe.Id == command.Id, cancellationToken);
        
        if (!recipeExists)
        {
            throw NotFoundException.Create<RecipeEntity>(command.Id);
        }
        
        recipeExists = await _recipesRepository.AnyAsync(recipe => recipe.CreatedBy == userId && recipe.Id == command.Id, cancellationToken);
        
        if (!recipeExists)
        {
            throw UnauthorizedException.Create();
        }

        var imageName = FileNameFormatter.FormatForRecipeImage(command.Id);
        var pdfName = FileNameFormatter.FormatForRecipePdf(command.Id);
        var ukrainianPdfName = FileNameFormatter.FormatForRecipeUkrainianPdf(command.Id);

        await Task.WhenAll(
            _fileStorageService.RemoveFileAsync(imageName, cancellationToken),
            _fileStorageService.RemoveFileAsync(pdfName, cancellationToken),
            _fileStorageService.RemoveFileAsync(ukrainianPdfName, cancellationToken)
        );

        await _recipesRepository.RemoveAsync(command.Id, cancellationToken: cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}