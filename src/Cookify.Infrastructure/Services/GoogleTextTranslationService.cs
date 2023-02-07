using Cookify.Application.Services;
using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Cookify.Infrastructure.Services;

public class GoogleTextTranslationService : ITextTranslationService
{
    private readonly TranslationClient _translationClient;
    private readonly ILogger<GoogleTextTranslationService> _logger;

    public GoogleTextTranslationService(TranslationClient translationClient, ILogger<GoogleTextTranslationService> logger)
    { 
        _translationClient = translationClient;
        _logger = logger;
    }
    
    public async Task<string> TranslateAsync(string sourceText, string sourceLanguage, string targetLanguage, CancellationToken cancellationToken)
    { 
        _logger.LogInformation("Translating text: {Text}", sourceText);
        
        var translationResult = await _translationClient.TranslateTextAsync(sourceText, targetLanguage, sourceLanguage, cancellationToken: cancellationToken);
        
        _logger.LogInformation("Text translated: {Text}", sourceText);
        
        return translationResult.TranslatedText;
    }
}