using Cookify.Application.Services;
using Cookify.Domain.Common.UnitOfWork;
using Cookify.Domain.Ingredient;
using Cookify.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Cookify.Infrastructure.Scheduling.Jobs;

[DisallowConcurrentExecution]
public class IngredientsSearcherJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIngredientsRepository _ingredientsRepository;
    private readonly ITextTranslationService _textTranslationService;
    private readonly SilpoProductMarketService _silpoProductMarketService; 
    private readonly ILogger<IngredientsSearcherJob> _logger;


    public IngredientsSearcherJob(
        IUnitOfWork unitOfWork, 
        ITextTranslationService textTranslationService,
        IIngredientsRepository ingredientsRepository,
        SilpoProductMarketService silpoProductMarketService,
        ILogger<IngredientsSearcherJob> logger
        )
    {
        _unitOfWork = unitOfWork;
        _textTranslationService = textTranslationService;
        _ingredientsRepository = ingredientsRepository;
        _silpoProductMarketService = silpoProductMarketService;
        _logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var ingredients = await _ingredientsRepository.WhereAsync();
        foreach (var ingredient in ingredients)
        {
            var marketProduct = await _silpoProductMarketService.GetProductAsync(ingredient.UkrainianName);
     
            break;
        }
    }
}