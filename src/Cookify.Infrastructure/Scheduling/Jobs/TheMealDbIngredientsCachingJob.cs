using AngleSharp;
using Cookify.Application.Common.Constants;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Expressions;
using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Ingredient;
using Cookify.Infrastructure.Services.RestApis;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Cookify.Infrastructure.Scheduling.Jobs;

[DisallowConcurrentExecution]
public class TheMealDbIngredientsCachingJob : IJob
{
    private readonly ITheMealDbApi _theMealDbApi;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIngredientsRepository _ingredientsRepository;
    private readonly ITextTranslationService _textTranslationService;
    private readonly ILogger<TheMealDbIngredientsCachingJob> _logger;
    private readonly IImageSearcherService _imageSearcherService;
    private readonly IInternetFileDownloaderService _internetFileDownloaderService;
    private readonly IFileStorageService _fileStorageService;
    private static readonly SemaphoreSlim SemaphoreSlim = new(20);

    public TheMealDbIngredientsCachingJob(
        ITheMealDbApi theMealDbApi, 
        IUnitOfWork unitOfWork, 
        IIngredientsRepository ingredientsRepository,
        ITextTranslationService textTranslationService,
        ILogger<TheMealDbIngredientsCachingJob> logger,
        IImageSearcherService imageSearcherService,
        IInternetFileDownloaderService internetFileDownloaderService,
        IFileStorageService fileStorageService
    )
    {
        _theMealDbApi = theMealDbApi;
        _unitOfWork = unitOfWork;
        _ingredientsRepository = ingredientsRepository;
        _textTranslationService = textTranslationService;
        _logger = logger;
        _imageSearcherService = imageSearcherService;
        _internetFileDownloaderService = internetFileDownloaderService;
        _fileStorageService = fileStorageService;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var responseDto = await _theMealDbApi.GetIngredientsAsync(context.CancellationToken);

        var existedIngredients = await _ingredientsRepository.WhereAsync(new []
        {
            IngredientExpressions.CreateByEquals(null, false)
        });
        var existedIngredientsNames = existedIngredients.Select(ingredient => ingredient.Name.ToLower());
        var missingIngredients = responseDto.Ingredients.Where(dto => !existedIngredientsNames.Contains(dto.Name.ToLower())).ToArray(); 

        var selectIngredientsTasks = missingIngredients.Select(async dto =>
        {
            try
            {
                await SemaphoreSlim.WaitAsync(context.CancellationToken);
                
                var translateNameAsyncTask = _textTranslationService.TranslateAsync( 
                    sourceText: dto.Name, 
                    sourceLanguage: TranslatingConstants.EnglishLanguage, 
                    targetLanguage: TranslatingConstants.UkrainianLanguage,
                    cancellationToken: context.CancellationToken
                    );
                
                var firstImageAsyncTask = _imageSearcherService.FirstImageAsync(dto.Name, context.CancellationToken);
                Task<string>? translateDescriptionAsyncTask = null;
            
                if (!string.IsNullOrWhiteSpace(dto.Description))
                {
                    translateDescriptionAsyncTask = _textTranslationService.TranslateAsync( 
                        sourceText: dto.Description, 
                        sourceLanguage: TranslatingConstants.EnglishLanguage, 
                        targetLanguage: TranslatingConstants.UkrainianLanguage,
                        cancellationToken: context.CancellationToken
                        );
                }

                var ukrainianDescription = translateDescriptionAsyncTask is null ? null : await translateDescriptionAsyncTask;
                var ukrainianName = await translateNameAsyncTask;
                var imageLink = await firstImageAsyncTask;

                var ingredientEntity = new IngredientEntity(
                    dto.Name,
                    ukrainianName,
                    description: dto.Description,
                    ukrainianDescription: ukrainianDescription
                );

                if (imageLink is null)
                {
                    return ingredientEntity;
                }
            
                await using var imageStream = await _internetFileDownloaderService.DownloadAsync(new Uri(imageLink), context.CancellationToken);
            
                var imageName = FileNameFormatter.FormatForIngredientImage(ingredientEntity.Id);
                await _fileStorageService.PutFileAsync(new FileModel(imageStream, ContentTypesConstants.ImageJpeg, imageName), context.CancellationToken);
                ingredientEntity.ImageLink = _fileStorageService.GetFileLink(imageName);
            
                return ingredientEntity;
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        });

        var ingredients = await Task.WhenAll(selectIngredientsTasks);

        try
        {
            await _ingredientsRepository.AddRangeAsync(ingredients, context.CancellationToken);
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogWarning("{ExceptionMessage} {StackTrace}", exception.Message, exception.StackTrace);
        }
    }
}