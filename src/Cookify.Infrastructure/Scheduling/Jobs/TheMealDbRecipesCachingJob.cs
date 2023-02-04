using Cookify.Application.Common.Helpers;
using Cookify.Application.Expressions;
using Cookify.Application.Models;
using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Ingredient;
using Cookify.Domain.IngredientRecipe;
using Cookify.Domain.Recipe;
using Cookify.Domain.RecipeCategory;
using Cookify.Infrastructure.Dtos.TheMealDb;
using Cookify.Infrastructure.Services.RestApis;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Cookify.Infrastructure.Scheduling.Jobs;

[DisallowConcurrentExecution]
public class TheMealDbRecipesCachingJob : IJob
{
    private static readonly SemaphoreSlim SemaphoreSlim = new(1);
    
    private const uint TheMealDbRecipeFirstId = 52800;
    private const uint TheMealDbRecipeLastId = 53000;
    
    private readonly ITheMealDbApi _theMealDbApi;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRecipesRepository _recipesRepository;
    private readonly IIngredientsRepository _ingredientsRepository;
    private readonly IRecipeCategoriesRepository _recipeCategoriesRepository;
    private readonly ITextTranslationService _textTranslationService;
    private readonly ILogger<TheMealDbRecipesCachingJob> _logger;
    private readonly IInternetFileDownloaderService _internetFileDownloaderService;
    private readonly IFileStorageService _fileStorageService;
    
    public TheMealDbRecipesCachingJob(
        ITheMealDbApi theMealDbApi, 
        IUnitOfWork unitOfWork, 
        IRecipesRepository recipesRepository,
        IRecipeCategoriesRepository recipeCategoriesRepository,
        IIngredientsRepository ingredientsRepository,
        ITextTranslationService textTranslationService,
        ILogger<TheMealDbRecipesCachingJob> logger,
        IInternetFileDownloaderService internetFileDownloaderService,
        IFileStorageService fileStorageService
    )
    {
        _theMealDbApi = theMealDbApi;
        _unitOfWork = unitOfWork;
        _recipesRepository = recipesRepository;
        _ingredientsRepository = ingredientsRepository;
        _recipeCategoriesRepository = recipeCategoriesRepository;
        _textTranslationService = textTranslationService;
        _logger = logger;
        _internetFileDownloaderService = internetFileDownloaderService;
        _fileStorageService = fileStorageService;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var getRecipeAsyncTasks = new List<Task<RecipeDto?>>();
        
        for (var id = TheMealDbRecipeFirstId; id < TheMealDbRecipeLastId; id++)
        {
            var idCopy = id;
            
            getRecipeAsyncTasks.Add(Task.Run(async () =>
            {
                var response = await _theMealDbApi.GetRecipeAsync(idCopy);
                
                return response.Recipes?.FirstOrDefault();
            }));
        }

        RecipeDto[] recipeDtos = (await Task.WhenAll(getRecipeAsyncTasks)).Where(recipe => recipe is not null).ToArray()!;

        var existedRecipes = await _recipesRepository.WhereAsync(recipe => recipe.CreatedBy == null);
        var existedRecipesTitles = existedRecipes.Select(recipe => recipe.Title.ToLower());
        var missingRecipes = recipeDtos.Where(dto => !existedRecipesTitles.Contains(dto.Name.ToLower())).ToArray(); 
        
        var selectRecipesTasks = missingRecipes.Select(async dto =>
        {
            try
            { 
                await SemaphoreSlim.WaitAsync();

                var translateNameAsyncTask = _textTranslationService.TranslateAsync(dto.Name, "en", "uk");
                var downloadAsyncTask = _internetFileDownloaderService.DownloadAsync(new Uri(dto.ImageLink));
                var translateInstructionAsyncTask = _textTranslationService.TranslateAsync(dto.Instruction, "en", "uk");
                var firstAsyncTask = _recipeCategoriesRepository.FirstAsync(category => category.Name.ToLower() == dto.CategoryName.ToLower());

                var ukrainianInstruction = await translateInstructionAsyncTask;
                var ukrainianName = await translateNameAsyncTask;
                var recipeCategory = await firstAsyncTask;
                await using var imageStream = await downloadAsyncTask;
            
                var ingredientsAndMeasures = GetIngredientsAndMeasures(recipeDtos.First());

                var recipeEntity = new RecipeEntity(
                    dto.Name,
                    ukrainianName,
                    dto.Instruction, 
                    ukrainianInstruction,
                    recipeCategory.Id
                );

                foreach (var (measure, ingredientName) in ingredientsAndMeasures)
                {
                    var ingredientEntity = await _ingredientsRepository.FirstAsync(IngredientExpressions.NameEquals(ingredientName));
                    var ukrainianMeasure = await _textTranslationService.TranslateAsync(measure, "en", "uk");
                    recipeEntity.IngredientRecipes.Add(new IngredientRecipeEntity(
                        ingredientEntity.Id, 
                        recipeEntity.Id, 
                        measure,
                        ukrainianMeasure
                        ));
                }

                var imageName = FileNameFormatter.FormatForRecipeImage(recipeEntity.Id);
                var contentType = FileExtensionsParser.ParseFromLink(dto.ImageLink);
                await _fileStorageService.PutFileAsync(new FileModel(imageStream, contentType, imageName));
                recipeEntity.ImageLink = _fileStorageService.GetFileLink(imageName);

                return recipeEntity;
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        });
        
        var recipes = await Task.WhenAll(selectRecipesTasks);

        try
        {
            await _recipesRepository.AddRangeAsync(recipes);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogWarning("{ExceptionMessage} {StackTrace}", exception.Message, exception.StackTrace);
        }
    }

    private static HashSet<(string, string)> GetIngredientsAndMeasures(RecipeDto recipeDto)
    {
        var existedIngredients = new HashSet<string>();
        var ingredientsAndMeasures = new HashSet<(string, string)>();
        
        for (var i = 1; i <= 20; i++)
        {
            var measure = (string)typeof(RecipeDto).GetProperty($"Measure{i}")!.GetValue(recipeDto)!;
            var ingredient = (string)typeof(RecipeDto).GetProperty($"Ingredient{i}")!.GetValue(recipeDto)!;

            if (string.IsNullOrWhiteSpace(measure) || string.IsNullOrWhiteSpace(ingredient))
            {
                continue;
            }

            if (existedIngredients.Contains(ingredient))
            {
                continue;
            }

            existedIngredients.Add(ingredient);
            ingredientsAndMeasures.Add((measure, ingredient));
        }

        return ingredientsAndMeasures;
    }
}