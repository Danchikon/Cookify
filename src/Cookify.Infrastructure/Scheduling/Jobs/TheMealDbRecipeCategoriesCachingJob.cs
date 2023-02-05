using System.Xml.XPath;
using Cookify.Application.Common.Constants;
using Cookify.Application.Common.Helpers;
using Cookify.Application.Expressions;
using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.MealCategory;
using Cookify.Domain.RecipeCategory;
using Cookify.Infrastructure.Services.RestApis;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Cookify.Infrastructure.Scheduling.Jobs;

[DisallowConcurrentExecution]
public class TheMealDbRecipeCategoriesCachingJob : IJob
{
    private readonly ITheMealDbApi _theMealDbApi;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRecipeCategoriesRepository _recipeCategoriesRepository;
    private readonly ITextTranslationService _textTranslationService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IInternetFileDownloaderService _internetFileDownloaderService;
    private readonly ILogger<TheMealDbRecipeCategoriesCachingJob> _logger;
    private static readonly SemaphoreSlim SemaphoreSlim = new(15);

    public TheMealDbRecipeCategoriesCachingJob(
        ITheMealDbApi theMealDbApi, 
        IUnitOfWork unitOfWork, 
        IRecipeCategoriesRepository recipeCategoriesRepository,
        ITextTranslationService textTranslationService,
        IFileStorageService fileStorageService,
        IInternetFileDownloaderService internetFileDownloaderService,
        ILogger<TheMealDbRecipeCategoriesCachingJob> logger
        )
    {
        _theMealDbApi = theMealDbApi;
        _unitOfWork = unitOfWork;
        _recipeCategoriesRepository = recipeCategoriesRepository;
        _textTranslationService = textTranslationService;
        _fileStorageService = fileStorageService;
        _internetFileDownloaderService = internetFileDownloaderService;
        _logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var responseDto = await _theMealDbApi.GetRecipeCategoriesAsync();

        var existedCategories = await _recipeCategoriesRepository.WhereAsync(new []
        {
            RecipeCategoryExpressions.CreateByEquals(null, false)
        });
        var existedCategoriesNames = existedCategories.Select(category => category.Name.ToLower());
        var missingCategories = responseDto.Categories.Where(dto => !existedCategoriesNames.Contains(dto.Name.ToLower())).ToArray();

        var selectCategoriesTasks = missingCategories.Select(async dto =>
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                
                var translateNameAsyncTask = _textTranslationService.TranslateAsync( 
                    sourceText: dto.Name, 
                    sourceLanguage: TranslatingConstants.EnglishLanguage, 
                    targetLanguage: TranslatingConstants.UkrainianLanguage
                    );
                
                var downloadAsyncTask = _internetFileDownloaderService.DownloadAsync(new Uri(dto.ImageLink));
                Task<string>? translateDescriptionAsyncTask = null;

                if (!string.IsNullOrWhiteSpace(dto.Description))
                {
                    translateDescriptionAsyncTask = _textTranslationService.TranslateAsync( 
                        sourceText: dto.Description, 
                        sourceLanguage: TranslatingConstants.EnglishLanguage, 
                        targetLanguage: TranslatingConstants.UkrainianLanguage
                        );
                }

                var ukrainianDescription = translateDescriptionAsyncTask is null ? null : await translateDescriptionAsyncTask;
                var ukrainianName = await translateNameAsyncTask;
                await using var imageStream = await downloadAsyncTask;

                var recipeCategoryEntity = new RecipeCategoryEntity(
                    dto.Name,
                    ukrainianName,
                    description: dto.Description,
                    ukrainianDescription: ukrainianDescription
                );
                
                var imageName = FileNameFormatter.FormatForRecipeCategoryImage(recipeCategoryEntity.Id);
                var contentType = FileExtensionsParser.ParseFromLink(dto.ImageLink);
                await _fileStorageService.PutFileAsync(new FileModel(imageStream, contentType, imageName));
                recipeCategoryEntity.ImageLink = _fileStorageService.GetFileLink(imageName);

                return recipeCategoryEntity;
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        });

        var categories = await Task.WhenAll(selectCategoriesTasks);

        try
        {
            await _recipeCategoriesRepository.AddRangeAsync(categories);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogWarning("{ExceptionMessage} {StackTrace}", exception.Message, exception.StackTrace);
        }
    }
}