using System.Text;
using Cookify.Application.Common.Constants;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Expressions;
using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Domain.Common.Entities;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Recipe;
using Cookify.Infrastructure.Common.Constants;
using HandlebarsDotNet;
using IronPdf;
using IronPdf.Rendering.Abstractions;
using Quartz;

namespace Cookify.Infrastructure.Scheduling.Jobs;

[DisallowConcurrentExecution]
public class RecipePdfGeneratorJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRecipesRepository _recipesRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly BasePdfRenderer _pdfRenderer;
    
    public RecipePdfGeneratorJob(
        IUnitOfWork unitOfWork, 
        IRecipesRepository recipesRepository,
        IFileStorageService fileStorageService,
        BasePdfRenderer pdfRenderer
    )
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _recipesRepository = recipesRepository;
        _pdfRenderer = pdfRenderer;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var recipes = await _recipesRepository.WherePdfLinkIsNullAsync();
        
        var ukrainianTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PdfTemplatesConstants.RecipeUkrainianPdfTemplatePath);
        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PdfTemplatesConstants.RecipePdfTemplatePath);
        var template = Handlebars.Compile(await File.ReadAllTextAsync(templatePath, Encoding.UTF8));
        var ukrainianTemplate = Handlebars.Compile(await File.ReadAllTextAsync(ukrainianTemplatePath, Encoding.UTF8));

        foreach (var recipeEntity in recipes)
        {
            using var ukrainianPdf = _pdfRenderer.RenderHtmlAsPdf(ukrainianTemplate(new
            {
                UkrainianTitle = recipeEntity.UkrainianTitle,
                CreatedAt = recipeEntity.CreatedAt.ToString("yyyy-MM-dd"),
                CategoryUkrainianName = recipeEntity.Category.UkrainianName,
                IngredientRecipes = recipeEntity.IngredientRecipes,
                UkrainianInstruction = recipeEntity.UkrainianInstruction,
                ImageLink = recipeEntity.ImageLink
            }));
            
            using var pdf = _pdfRenderer.RenderHtmlAsPdf(template(new
            {
                Title = recipeEntity.Title,
                CreatedAt = recipeEntity.CreatedAt.ToString("yyyy-MM-dd"),
                CategoryName = recipeEntity.Category.Name,
                IngredientRecipes = recipeEntity.IngredientRecipes,
                Instruction = recipeEntity.Instruction,
                ImageLink = recipeEntity.ImageLink
            }));
            
            var ukrainianFileName = FileNameFormatter.FormatForRecipeUkrainianPdf(recipeEntity.Id);
            var fileName = FileNameFormatter.FormatForRecipePdf(recipeEntity.Id);
            
            var ukrainianPdfLink = await _fileStorageService.PutFileAsync(new FileModel(
                ukrainianPdf.Stream, 
                FileExtensionsConstants.ApplicationPdf,
                ukrainianFileName)
            );
            
            var pdfLink = await _fileStorageService.PutFileAsync(new FileModel(
                pdf.Stream, 
                FileExtensionsConstants.ApplicationPdf, 
                fileName)
            );
            
            var partialRecipe = new PartialEntity<RecipeEntity>();
            partialRecipe.AddValue(recipe => recipe.UkrainianPdfLink, ukrainianPdfLink);
            partialRecipe.AddValue(recipe => recipe.PdfLink, pdfLink);
            await _recipesRepository.PartiallyUpdateAsync(recipeEntity.Id, partialRecipe);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}